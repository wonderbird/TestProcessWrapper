using System;
using TechTalk.SpecFlow;
using Xunit;

namespace kata_rabbitmq.bdd.tests.Steps
{
    [Binding]
    public class FreshSystemStepDefinitions
    {
        private bool _isRobotExchangePresent = false;

        [Given("a fresh system is installed")]
        public void GivenAFreshSystemIsInstalled()
        {
        }
        
        [Then("the RabbitMQ channel is open")]
        public void TheRabbitMqChannelIsOpen()
        {
            Assert.True(RabbitMq.Channel.IsOpen);
        }
        
        [When("robot exchanges are queried")]
        public void WhenRobotExchangesAreQueried()
        {
            try
            {
                RabbitMq.Channel.ExchangeDeclarePassive("robot");
                _isRobotExchangePresent = true;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [Then("the robot exchanges do not exist")]
        public void TheRobotExchangesDoNotExist()
        {
            Assert.False(_isRobotExchangePresent);
        }
    }
}
