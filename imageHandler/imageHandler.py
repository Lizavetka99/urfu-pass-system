import numpy
import cv2
import os
import face_recognizer, image_cutter, quality_checker, transparency_remover, \
    image_rotator, blur_checker

class ImageHandler:
    def __init__(self):
        self.face_recognizer = face_recognizer.FaceRecognizer()
        self.image_cutter = image_cutter.ImageCutter()
        self.quality_checker = quality_checker.QualityChecker()
        self.transparency_remover = transparency_remover.TransparencyRemover()
        self.image_rotator = image_rotator.ImageRotator()
        self.blur_checker = blur_checker.BlurChecker()

    def edit_image(self,  image : numpy.ndarray) -> numpy.ndarray:
        if not self.quality_checker.check_for_min_quality(image):
            raise ValueError("Изображение должно быть минимум 300x400")

        image_without_transparency = self.transparency_remover.remove_transparancy(image)
        image_rotated_by_face = self.image_rotator.rotate(image_without_transparency)
        image_face_cut = self.face_recognizer.recognize(image_rotated_by_face)
        if (not self.blur_checker.check_for_blur(image_face_cut)):
            raise ValueError('Изображение слишком размыто')
        image_3x4 = self.image_cutter.cut_3x4(image_face_cut)
        compressed_image = self.image_cutter.cut_quality(image_3x4)
        self.face_recognizer.recognize(compressed_image)
        return compressed_image

    def edit_image_file(self, input_file, output_file):
        if not os.path.exists(os.path.dirname(output_file)):
            os.makedirs(os.path.dirname(output_file))
        if os.path.isfile(input_file):
            image = cv2.imread(input_file, cv2.IMREAD_UNCHANGED)
            try:
                edited_image = self.edit_image(image)
                cv2.imwrite(output_file, edited_image, [cv2.IMWRITE_JPEG_QUALITY, 100])
            except Exception as e:
                return f'ValueError: {e}'
        else:
            return f'Файл {input_file} не найден'

    def edit_images_folder(self, input_directory, output_directory):
        messages = dict()
        if not os.path.exists(output_directory):
            os.makedirs(output_directory)

        for filename in os.listdir(input_directory):
            input_path = os.path.join(input_directory, filename)
            output_path = os.path.join(output_directory, filename)
            try:
                self.edit_image_file(input_path, output_path)
            except ValueError as e:
                messages[filename] = e
        return messages



