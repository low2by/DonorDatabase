using System;

namespace ContactInfomation
{
    public class ContactInformation
    {
        public ContactInformation(string _lastName, string _firstName, string _email, string _phoneNumber)
        {
            LastName = _lastName;
            FirstName = _firstName;
            Email = _email;
            PhoneNumber = _phoneNumber;
        }

        public string LastName { get; }
        public string FirstName { get; }
        public string Email { get; }
        public string PhoneNumber { get; }
    }
}
