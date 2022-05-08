package pg.rsww.redteam.customers.services;

import lombok.experimental.StandardException;

@StandardException
public class InvalidAuthenticationTokenException extends Exception {
    public InvalidAuthenticationTokenException(String message) {
        super(message, null);
    }
}
