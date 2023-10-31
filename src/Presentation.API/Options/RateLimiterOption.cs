namespace WhatIBoughtAPI.Options;

public class RateLimiterOption
{
    public int PermitLimit { get; set; } = 10;
    public int WindowSeconds { get; set; } = 10;
    public int QueueLimit { get; set; } = 4;
}