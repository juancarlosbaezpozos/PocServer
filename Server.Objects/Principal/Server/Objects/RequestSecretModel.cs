namespace Principal.Server.Objects
{
    public class RequestSecretModel
    {
        public string Aws_Secret_Id { get; set; }
        public string Aws_Secret_Key { get; set; }
        public string SecretName { get; set; }
    }
}
