using System;
using System.Security.Cryptography;

namespace NCuid
{
    /// <summary>
    /// When generating CUIDs, specifes which kind of random number source will be used. 
    /// </summary>
    public enum RandomSource
    {
        /// <summary>
        /// Numbers generated will emanate from <see cref="Random"/>
        /// </summary>
        Simple,
        /// <summary>
        /// Number generated will emanate from the slower and more secure <see cref="RNGCryptoServiceProvider"/>
        /// </summary>
        Secure
    }
}