package pg.rsww.redteam.payments.services;

public class CardPaymentException extends Exception {
    public CardPaymentException(String msg) {
        super(msg);
    }
}
