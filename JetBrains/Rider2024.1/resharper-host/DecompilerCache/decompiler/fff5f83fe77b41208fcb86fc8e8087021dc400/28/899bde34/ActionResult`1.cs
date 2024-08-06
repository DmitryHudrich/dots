// Decompiled with JetBrains decompiler
// Type: Microsoft.AspNetCore.Mvc.ActionResult`1
// Assembly: Microsoft.AspNetCore.Mvc.Core, Version=8.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// MVID: FFF5F83F-E77B-4120-8FCB-86FC8E808702
// Assembly location: /usr/share/dotnet/shared/Microsoft.AspNetCore.App/8.0.6/Microsoft.AspNetCore.Mvc.Core.dll

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Core;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;

#nullable enable
namespace Microsoft.AspNetCore.Mvc
{
  public sealed class ActionResult<TValue> : IConvertToActionResult
  {
    private const int DefaultStatusCode = 200;

    public ActionResult(TValue value)
    {
      if (typeof (IActionResult).IsAssignableFrom(typeof (TValue)) || typeof (IResult).IsAssignableFrom(typeof (TValue)))
        throw new ArgumentException(Resources.FormatInvalidTypeTForActionResultOfT((object) typeof (TValue), (object) "ActionResult<T>"));
      this.Value = value;
    }

    public ActionResult(ActionResult result)
    {
      if (typeof (IActionResult).IsAssignableFrom(typeof (TValue)) || typeof (IResult).IsAssignableFrom(typeof (TValue)))
        throw new ArgumentException(Resources.FormatInvalidTypeTForActionResultOfT((object) typeof (TValue), (object) "ActionResult<T>"));
      this.Result = result ?? throw new ArgumentNullException(nameof (result));
    }

    public ActionResult? Result { get; }

    public TValue? Value { get; }

    public static implicit operator ActionResult<TValue>(TValue value)
    {
      return new ActionResult<TValue>(value);
    }

    public static implicit operator ActionResult<TValue>(ActionResult result)
    {
      return new ActionResult<TValue>(result);
    }

    #nullable disable
    IActionResult IConvertToActionResult.Convert()
    {
      if (this.Result != null)
        return (IActionResult) this.Result;
      int num = !(this.Value is ProblemDetails problemDetails) || !problemDetails.Status.HasValue ? 200 : problemDetails.Status.Value;
      return (IActionResult) new ObjectResult((object) this.Value)
      {
        DeclaredType = typeof (TValue),
        StatusCode = new int?(num)
      };
    }
  }
}
