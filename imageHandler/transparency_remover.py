import numpy


class TransparencyRemover:
    def remove_transparancy(self, image : numpy.ndarray) -> numpy.ndarray:
        """Возвращает изображение без прозрачности"""
        if (len(image.shape) == 2):
            raise ValueError("Изображение черно-белое")
        if image.shape[2] == 4:
            print(f"Изображение имеет альфа-канал (прозрачность).")
            alpha_channel = image[:, :, 3]
            mask = alpha_channel == 0
            # Заменяем прозрачные пиксели на белый цвет
            image[mask] = [255, 255, 255, 255]
            # Удаляем альфа-канал
            image = image[:, :, :3]
        else:
            print(f"Изображение не имеет альфа-канала (прозрачности).")
        return image