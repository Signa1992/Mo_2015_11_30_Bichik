namespace Mo_2015_11_30_Bichik
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class Data 
    {
        [Serializable]
        public class Page
        {
            public string Name;
            public string ImagePath;
        }

        public List<Page> Pages;
    }
}