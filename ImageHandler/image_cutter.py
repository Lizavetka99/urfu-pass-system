import cv2
import os

import numpy


class ImageCutter:
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
