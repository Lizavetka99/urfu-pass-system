import cv2
import os

import numpy


class ImageCutter:
    def cut_in_proportions(self, image : numpy.ndarray, face_square): # передается cv изображение
        # Обнаружение лиц
        x, y, w, h = face_square
        # cv2.rectangle(img, (x, y), (x+w, y+h), (255, 0, 0), 2)
        new_x = max(0, x - w * 0.5)
        new_y = max(0, y - h * 0.5)
        new_xw = min(image.shape[1], x + w * 1.5)
        new_yh = min(image.shape[0], y + h * 1.5)
        if new_xw >= image.shape[1]:
            new_xw = image.shape[1] - 1
        if new_yh >= image.shape[0]:
            new_yh = image.shape[0] - 1
        img = image[int(new_y):int(new_yh), int(new_x):int(new_xw)]
        return img

    def cut_3x4(self, image : numpy.ndarray) -> numpy.ndarray:
        # Изменяем размер изображения
        height = image.shape[0] // 4
        width = image.shape[1] // 3
        part = min(height, width)
        delta_y = (image.shape[0] - part * 4) // 2
        delta_x = (image.shape[1] - part * 3) // 2
        img = image[delta_y : delta_y + part * 4, delta_x : delta_x + part * 3]
        #img = cv2.resize(img, (part * 3, part * 4))
        return img

    def cut_quality(self, image : numpy.ndarray) -> numpy.ndarray:
        resized_img = cv2.resize(image, (900, 1200))
        return resized_img
