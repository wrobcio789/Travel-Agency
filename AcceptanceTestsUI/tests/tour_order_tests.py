import unittest

from selenium import webdriver

from base_selenium import BaseSelenium
from tour import *


class TourOrderTests(unittest.TestCase):
    def test_positive_make_order_and_valid_payment(self):
        is_payment_valid = True
        is_bought = self.make_order(is_payment_valid)
        self.assertEqual(is_payment_valid, is_bought)

    def test_negative_make_order_and_invalid_payment(self):
        is_payment_valid = False
        is_bought = self.make_order(is_payment_valid)
        self.assertEqual(is_payment_valid, is_bought)

    @staticmethod
    def make_order(successful_payment=True):
        url = "http://localhost:8099"
        username, password = "admin", "admin"
        chrome_path = r"./../../Scrapper/src/scrapper/webdriver/chromedriver.exe"
        driver = BaseSelenium(webdriver.Chrome(chrome_path))
        driver.open(url)
        login(driver, username, password)
        choose_offer(driver)
        choose_transport(driver)
        choose_hotel(driver)
        check_offer_availability(driver)
        buy_offer(driver)
        pay(driver, successful_payment)
        is_bought = is_order_bought(driver)
        return is_bought


if __name__ == '__main__':
    unittest.main()
