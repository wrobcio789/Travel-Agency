package pg.rsww.redteam.payments.services;

import pg.rsww.redteam.payments.models.Amount;
import pg.rsww.redteam.payments.models.CreditCardData;

public interface CardPaymentProvider {
    public void makePayment(CreditCardData cardData, Amount amount) throws CardPaymentException;
}
