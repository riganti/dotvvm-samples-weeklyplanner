using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DotVVM.Framework.Compilation.Javascript;
using DotVVM.Framework.Compilation.Javascript.Ast;

namespace WeeklyPlanner
{

    public class DateTimeAddDaysTranslator : IJavascriptMethodTranslator
    {
        public JsExpression TryTranslateCall(LazyTranslatedExpression context, LazyTranslatedExpression[] arguments, MethodInfo method)
        {
            var tmp = new JsTemporaryVariableParameter();

            return new JsBinaryExpression(
                new JsAssignmentExpression(new JsSymbolicParameter(tmp), new JsInvocationExpression(new JsIdentifierExpression("dotvvm").Member("globalize").Member("parseDotvvmDate"), context.JsExpression())),
                BinaryOperatorType.Sequence,
                new JsNewExpression("Date",
                    new JsInvocationExpression(new JsMemberAccessExpression(new JsSymbolicParameter(tmp), "getFullYear")),
                    new JsInvocationExpression(new JsMemberAccessExpression(new JsSymbolicParameter(tmp), "getMonth")),
                    new JsBinaryExpression(new JsInvocationExpression(new JsMemberAccessExpression(new JsSymbolicParameter(tmp), "getDate")), BinaryOperatorType.Plus, arguments[0].JsExpression()),
                    new JsInvocationExpression(new JsMemberAccessExpression(new JsSymbolicParameter(tmp), "getHours")),
                    new JsInvocationExpression(new JsMemberAccessExpression(new JsSymbolicParameter(tmp), "getMinutes")),
                    new JsInvocationExpression(new JsMemberAccessExpression(new JsSymbolicParameter(tmp), "getSeconds")),
                    new JsInvocationExpression(new JsMemberAccessExpression(new JsSymbolicParameter(tmp), "getMilliseconds"))
                )
            );
        }
    }

    public class DateTimeDayOfYearTranslator : IJavascriptMethodTranslator
    {
        public JsExpression TryTranslateCall(LazyTranslatedExpression context, LazyTranslatedExpression[] arguments, MethodInfo method)
        {
            return new JsInvocationExpression(
                new JsInvocationExpression(new JsIdentifierExpression("dotvvm").Member("globalize").Member("parseDotvvmDate"), context.JsExpression())
                    .Member("getDayOfYear")
            );

        }
    }
}
