using System;
using Xunit;
using TechTalk.SpecFlow;

namespace kata_rabbitmq.bdd.tests
{
    [Binding]
    public class FreshSystemStepDefinitions
    {
        private ScenarioContext _scenarioContext;
        
        FreshSystemStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given("a fresh system is installed")]
        public void GivenAFreshSystemIsInstalled()
        {
            _scenarioContext.Pending();
        }

        [When("exchanges are queried")]
        public void WhenExchangesAreQueried()
        {
            _scenarioContext.Pending();
        }

        [Then("there should exist only the RabbitMQ default exchanges")]
        public void ThenThereShouldExistOnlyTheRabbitMQDefaultExchanges()
        {
            _scenarioContext.Pending();
        }
    }
}
