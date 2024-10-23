def check_for_min_quality(image):
    """Возвращает True, если изображение подходит по качеству"""
    pass


def remove_transparency(image, input_path):
    """Возвращает изображение без прозрачности"""
    if image.shape[2] == 4:
        print(f"Изображение {input_path} имеет альфа-канал (прозрачность).")

        # Создаем маску для прозрачных пикселей
        alpha_channel = image[:, :, 3]
        mask = alpha_channel == 0

        # Заменяем прозрачные пиксели на белый цвет
        image[mask] = [255, 255, 255, 255]

        # Удаляем альфа-канал
        image = image[:, :, :3]

    else:
        print(f"Изображение {input_path} не имеет альфа-канала (прозрачности).")


def identify_face(image):
    """Возвращает True, если одно лицо на изображении"""
    pass


def turn_face(image):
    """Возвращает изображение лица в нужной ориентации"""
    pass


def check_for_blur(image):
    """Возвращает False, если изображение слишком размыто"""
    pass


def cut_quality(image):
    """Возвращает изображение, урезанное по качеству,
    если изображение слишком большое"""
    pass

def save_image(image):
    """Сохраняет изображение в папку"""
    pass
