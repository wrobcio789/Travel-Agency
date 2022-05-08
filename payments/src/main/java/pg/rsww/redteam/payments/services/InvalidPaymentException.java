package pg.rsww.redteam.payments.services;

public class InvalidPaymentException extends Exception {
    public InvalidPaymentException(String msg){
        super(msg);
    }
}
