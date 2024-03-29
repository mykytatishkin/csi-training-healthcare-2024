﻿namespace CSI.IBTA.Shared.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public Role Role { get; set; }
        public User? User { get; set; }
    }
}
