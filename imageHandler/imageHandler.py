import numpy
import cv2
import os
import face_recognizer, image_cutter, quality_checker, transparency_remover, \
    image_rotator, blur_checker

class ImageHandler:
    def __init__(self):
        self.input_directory = 'Humans_faces_dataset'
        self.output_directory = 'Output_images'
        self.face_recognizer = face_recognizer.FaceRecognizer()
        self.image_cutter = image_cutter.ImageCutter()
        self.quality_checker = quality_checker.QualityChecker()
        self.transparency_remover = transparency_remover.TransparencyRemover()
        self.image_rotator = image_rotator.ImageRotator()
        self.blur_checker = blur_checker.BlurChecker()

    def edit_image(self, image : numpy.ndarray) -> numpy.ndarray:
       # if not self.quality_checker.check_for_min_quality(image):
        #    raise ValueError("Изображение должно быть минимум 900x1200")
        image_without_transparency = self.transparency_remover.remove_transparancy(image)
        image_rotated_by_face = self.image_rotator.rotate(image_without_transparency)
        image_face_cut = self.face_recognizer.recognize(image_rotated_by_face)
        if (not self.blur_checker.check_for_blur(image_face_cut)):
            raise ValueError('Изображение слишком размыто')
        image_3x4 = self.image_cutter.cut_3x4(image_face_cut)
        compressed_image = self.image_cutter.cut_quality(image_3x4)
        return compressed_image

    def edit_images_folder(self):
        if not os.path.exists(self.output_directory):
            os.makedirs(self.output_directory)

        for filename in os.listdir(self.input_directory):
            # Полный путь к исходному файлу
            input_path = os.path.join(self.input_directory, filename)

            # Проверяем, что файл существует и является файлом
            if os.path.isfile(input_path):
                # Читаем изображение с поддержкой альфа-канала
                img = cv2.imread(input_path, cv2.IMREAD_UNCHANGED)
                # Проверяем, что изображение загружено корректно
                if img is not None:
                    try:
                        edited_image = self.edit_image(img)
                    except ValueError as e:
                        print(f'{filename} error: {e}')
                        continue
                    output_path = os.path.join(self.output_directory, filename)
                    cv2.imwrite(output_path, edited_image,
                                [cv2.IMWRITE_JPEG_QUALITY, 100])
                else:
                    print(f'Изображение {filename} не найдено')