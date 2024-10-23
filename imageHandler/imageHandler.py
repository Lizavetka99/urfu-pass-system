def check_for_min_quality(image):
    """Возвращает True, если изображение подходит по качеству"""
    height, width = image.shape[:2]
    return height >= 480 and width >= 360  # Данные размеры соответствуют >= 300 dpi для фото 3х4.


def remove_transparency(image):
    """Возвращает изображение без прозрачности"""
    pass


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


def rename_image(image_name):
    """Возвращает название изображения, соответствующее критериям"""
    pass


def save_image(image):
    """Сохраняет изображение в папку"""
    pass
