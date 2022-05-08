package pg.rsww.redteam.payments.services;

import org.springframework.stereotype.Component;
import pg.rsww.redteam.payments.models.Amount;
import pg.rsww.redteam.payments.models.CreditCardData;

@Component
public class MockCardPaymentProvider implements CardPaymentProvider {

    @Override
    public void makePayment(CreditCardData cardData, Amount amount) throws CardPaymentException{
        if(cardData.getCvCode() == 420) {
            throw new CardPaymentException("Insufficient balance");
        }
    }
}
