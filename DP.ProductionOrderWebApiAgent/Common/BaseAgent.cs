using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DP.ProductionOrderWebApiAgent.Helpers;

namespace DP.ProductionOrderWebApiAgent.Common
{
    public abstract class BaseAgent
    {

        #region Properties

        protected string ControllerName { get; private set; }

        internal readonly JsonHelper Proxy = new JsonHelper();

        #endregion

        public BaseAgent(string controllerName)
        {

            ControllerName = controllerName;
        }

        protected string CreateUrl(string actionName, object parameter)
        {
            return string.Format("{0}/{1}/{2}", ControllerName, actionName, parameter);
        }

        protected TResult Post<TFilter, TResult>(string actionName, TFilter filter)
        {
            var uri = string.Format("{0}/{1}", ControllerName, actionName);
            const string logText = "OPERATION STATUS: Url:{0} / ResultMessage:{1}";
            string returnMsg = "SUCCESS";

            try
            {
                return Proxy.Post<TFilter, TResult>(uri, filter);
            }
            catch (Exception e)
            {
                returnMsg = "FAILED";
                throw e;
            }
            finally
            {
                //Log(string.Format(logText, uri, returnMsg));
            }
        }

        protected void Log(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected TResult Log<TFilter, TResult>(Func<TFilter, TResult> action, TFilter filter)
        {
            try
            {
                return action.Invoke(filter);
            }
            catch (Exception e)
            {
                throw e;
            }
        }



    }
}
