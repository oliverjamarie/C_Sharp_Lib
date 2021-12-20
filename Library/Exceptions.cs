using System;

namespace Library.Graph
{
    public class ElementNotInGraphException : Exception
    {
        public ElementNotInGraphException() : base("Element Not Found In Graph")
        {
        }
    } 
}

namespace Library.Graph.Algorithms
{
    public class PrefAttachNullRootException : Exception
    {
        public PrefAttachNullRootException() :
            base("Cannot start PrefAttach if graph's root node is null")
        {

        }
    }

    public class PrefAttachInvalidParamException : Exception
    {
        public PrefAttachInvalidParamException() : base("Sum of costs must be between 0 and 1")
        {

        }
    }

    public class InvalidTunerParameterException : Exception
    {
        public InvalidTunerParameterException() : base("Input must be between 0.0 and 1.0")
        {

        }
    }
}
