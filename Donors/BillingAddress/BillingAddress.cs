using System;

namespace BillingAddress
{
    public class BillingAddress
    {
        public BillingAddress(string _country, string _address, string _city, string _state, string _zipCode)
        {
            Country = _country;
            CityAddress = _address;
            City = _city;
            State = _state;
            ZipCode = _zipCode;
        }

        public string Country { get; }
        public string CityAddress { get; }
        public string City { get; }
        public string State { get; }
        public string ZipCode { get; }
    }
}
