from selenium.webdriver.support.wait import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC


class BaseSelenium(object):
    def __init__(self, driver):
        self.driver = driver

    def open(self, url):
        self.driver.get(url)

    def locate_element(self, loc):
        try:
            print(loc)
            element = WebDriverWait(self.driver, 10).until(EC.visibility_of_element_located(loc))
            return element
        except:
            print(f"cannot find {loc} element")
        return None

    def wait_for_element_invisible(self, loc):
        try:
            WebDriverWait(self.driver, 10).until(EC.invisibility_of_element_located(loc))
            return True
        except:
            print(f"cannot invisibility_of_element {loc} element")
        return False

    def send_key_with_element(self, loc, value):
        self.locate_element(loc).clear()
        self.locate_element(loc).send_keys(value)

    def click_with_element(self, loc):
        self.locate_element(loc).click()

    def click_elements_by_send_key(self, loc, value):
        self.locate_element(loc).send_keys(value)
