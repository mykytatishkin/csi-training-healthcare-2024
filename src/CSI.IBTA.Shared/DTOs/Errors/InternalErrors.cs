﻿namespace CSI.IBTA.Shared.DTOs.Errors
{
    public static class InternalErrors
    {
        public static InternalError GenericError = new("Something went wrong...");
        public static InternalError InvalidToken = new("Token is invalid");
    }
}
