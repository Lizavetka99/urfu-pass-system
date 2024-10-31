class QualityChecker:
    def check_for_min_quality(self, image):
        """Возвращает True, если изображение подходит по качеству"""
        height, width = image.shape[:2]
        return height >= 400 and width >= 300  # Данные размеры соответствуют >= 300 dpi для фото 3х4.
