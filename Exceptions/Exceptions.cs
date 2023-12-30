namespace demoAsp2.Exceptions
{
    public abstract class Exceptions : Exception
    {


        public Exceptions(string message) : base(message)
        {


        }

    }


    public class ExceptionNotFound : Exceptions
    {
        public ExceptionNotFound(string message) : base(message)
        {


        }
    }

    public class ExceptionBadRequest : Exception
    {
        public ExceptionBadRequest(string message) : base(message)
        {
        }
    }



}




