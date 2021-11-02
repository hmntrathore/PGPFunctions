Sample snippet for PGP Encryption Decryption leveraging Azure Function and PGPCore. This can be used to Encrypt Decrypt from Logic App or any other source. There are two fuctions

SignandEncrypt This function API can be used for Signing and Encryption, support 3 methods which can be invoked based on query string input req. sign - to be used when only signing is required encrypt - to be used when only encryption is required default - to be used for both sign and encrypt
VerifyandDecrypt This function API can be used for Signing and Encryption, support 3 methods which can be invoked based on query string input req. verify -to be used when only verification is required decrypt -to be used when only decryption is required default - to be used when both decryption and verification is required
Certificates can be stored in Axure Key vault as Base64 string and will be accessed in function using Environment variable.

For generating keys for testing I have used https://pgptool.org/
