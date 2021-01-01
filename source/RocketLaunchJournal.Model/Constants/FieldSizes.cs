using System;
using System.Collections.Generic;
using System.Text;

namespace RocketLaunchJournal.Model.Constants
{
    public class FieldSizes
    {
        public const int PasswordMinimumLength = 12;

        public const int ClaimTypeLength = 500;
        public const int ClaimValueLength = 500;

        /// <summary>
        /// Must be equal to the largest of the communication method lengths
        /// </summary>
        public const int CommunicationMethodLength = 500;
        public const int EmailLength = 500;
        public const int PhoneLength = 25;

        public const int NameLength = 200;
        public const int DescriptionLength = 2000;

        public const int DocumentFilenameLength = 1000;
        public const int DocumentDisplayNameLength = 500;

        public const int ConcurrencyStampLength = 50;

        public const int AddressFieldLengths = 200;

        public const int IpAddressLength = 50;
    }
}
