namespace TestProcessWrapper
{
    internal interface IProcessFactory
    {
        public string AppProjectName { get; }

        public bool IsCoverletEnabled { get; set; }

        BuildConfiguration BuildConfiguration { get; set; }

        ITestProcess Build();
    }
}
