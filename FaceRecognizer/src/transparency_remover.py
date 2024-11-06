import numpy
import cv2

class TransparencyRemover:
    def remove_transparancy(self, image : numpy.ndarray) -> numpy.ndarray:
        """Возвращает изображение без прозрачности"""
        if (len(image.shape) == 2):
            gray_image = cv2.cvtColor(image, cv2.COLOR_GRAY2BGR)
            return gray_image
        if image.shape[2] == 4:
            alpha_channel = image[:, :, 3]
            mask = alpha_channel == 0
            # Заменяем прозрачные пиксели на белый цвет
            image[mask] = [255, 255, 255, 255]
            # Удаляем альфа-канал
            image = image[:, :, :3]
        return image