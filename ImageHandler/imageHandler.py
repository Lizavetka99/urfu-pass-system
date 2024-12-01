import numpy
import cv2
import os
import image_cutter, quality_checker, transparency_remover, \
    image_rotator, blur_checker, face_square_recogniser
import threading
import queue

class ImageHandler:
    def __init__(self):
        self.image_cutter = image_cutter.ImageCutter()
        self.quality_checker = quality_checker.QualityChecker()
        self.transparency_remover = transparency_remover.TransparencyRemover()
        self.image_rotator = image_rotator.ImageRotator()
        self.blur_checker = blur_checker.BlurChecker()
        self.face_square_recognizer = face_square_recogniser.FaceSquareRecognizer()
        self.messages = dict()
        self.max_threads = 8


    def edit_image(self,  image : numpy.ndarray) -> numpy.ndarray:
        if not self.quality_checker.check_for_min_quality(image):
            raise ValueError(1)

        image_without_transparency = self.transparency_remover.remove_transparancy(image)
        image_rotated = self.image_rotator.rotate(image_without_transparency)
        face_square = self.face_square_recognizer.get_face_rectangle(image_rotated)
        if (not self.blur_checker.check_for_blur(image_rotated, face_square)):
            raise ValueError(1)
        image_face_cut = self.image_cutter.cut_in_proportions(image_rotated, face_square)
        image_3x4 = self.image_cutter.cut_3x4(image_face_cut)
        compressed_image = self.image_cutter.cut_quality(image_3x4)
        self.face_square_recognizer.get_face_rectangle(compressed_image)
        return compressed_image

    def edit_image_file(self, input_file, output_file):
        if not os.path.exists(os.path.dirname(output_file)):
            os.makedirs(os.path.dirname(output_file))
        if os.path.isfile(input_file):
            image = cv2.imread(input_file, cv2.IMREAD_UNCHANGED)
            try:
                edited_image = self.edit_image(image)
                cv2.imwrite(output_file, edited_image, [cv2.IMWRITE_JPEG_QUALITY, 100])
            except ValueError as e:
                raise ValueError(e.args[0])

    def edit_images_folder(self, input_directory, output_directory):
        self.messages = dict()
        if not os.path.exists(output_directory):
            os.makedirs(output_directory)

        image_queue = queue.Queue()
        threads = []
        for i in range(self.max_threads):
            t = threading.Thread(target=self.worker, args=(image_queue,))
            t.start()
            threads.append(t)

        for filename in os.listdir(input_directory):
            if (not filename.lower().endswith(('.jpg', '.jpeg', '.png', '.jfif', '.heic'))):
                continue
            input_path = os.path.join(input_directory, filename)
            output_path = os.path.join(output_directory, filename)
            image_queue.put((filename, input_path, output_path))

        image_queue.join()

        for i in range(self.max_threads):
            image_queue.put(None)
        for t in threads:
            t.join()

        return self.messages

    def worker(self, image_queue):
        while True:
            data = image_queue.get()
            if data is None:
                break
            filename, input_path, output_path = data
            try:
                self.edit_image_file(input_path, output_path)
            except Exception as e:
                self.messages[filename] = e.args[0]
            finally:
                image_queue.task_done()
