def check_for_min_quality(image):
    """Возвращает True, если изображение подходит по качеству"""
    pass


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
    minimumQuality = 100

    #  Еще нужно тестить на множестве фотографий, желательно на квадратах лиц.
    #  Минимальное значение "чёткости" пока условно.

    gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    return int(cv2.Laplacian(gray, cv2.CV_64F).var()) >= minimumQuality


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
