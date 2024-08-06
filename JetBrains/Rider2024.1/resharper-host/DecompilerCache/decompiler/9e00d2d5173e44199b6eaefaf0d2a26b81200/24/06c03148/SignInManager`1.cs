// Decompiled with JetBrains decompiler
// Type: Microsoft.AspNetCore.Identity.SignInManager`1
// Assembly: Microsoft.AspNetCore.Identity, Version=8.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// MVID: 9E00D2D5-173E-4419-9B6E-AEFAF0D2A26B
// Assembly location: /usr/share/dotnet/shared/Microsoft.AspNetCore.App/8.0.6/Microsoft.AspNetCore.Identity.dll

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace Microsoft.AspNetCore.Identity
{
  public class SignInManager<TUser> where TUser : class
  {
    #nullable disable
    private const string LoginProviderKey = "LoginProvider";
    private const string XsrfKey = "XsrfId";
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IAuthenticationSchemeProvider _schemes;
    private readonly IUserConfirmation<TUser> _confirmation;
    private HttpContext _context;
    private SignInManager<TUser>.TwoFactorAuthenticationInfo _twoFactorInfo;

    #nullable enable
    public SignInManager(
      Microsoft.AspNetCore.Identity.UserManager<TUser> userManager,
      IHttpContextAccessor contextAccessor,
      IUserClaimsPrincipalFactory<TUser> claimsFactory,
      IOptions<IdentityOptions> optionsAccessor,
      ILogger<SignInManager<TUser>> logger,
      IAuthenticationSchemeProvider schemes,
      IUserConfirmation<TUser> confirmation)
    {
      ArgumentNullException.ThrowIfNull((object) userManager, nameof (userManager));
      ArgumentNullException.ThrowIfNull((object) contextAccessor, nameof (contextAccessor));
      ArgumentNullException.ThrowIfNull((object) claimsFactory, nameof (claimsFactory));
      this.UserManager = userManager;
      this._contextAccessor = contextAccessor;
      this.ClaimsFactory = claimsFactory;
      this.Options = optionsAccessor?.Value ?? new IdentityOptions();
      this.Logger = (ILogger) logger;
      this._schemes = schemes;
      this._confirmation = confirmation;
    }

    public virtual ILogger Logger { get; set; }

    public Microsoft.AspNetCore.Identity.UserManager<TUser> UserManager { get; set; }

    public IUserClaimsPrincipalFactory<TUser> ClaimsFactory { get; set; }

    public IdentityOptions Options { get; set; }

    public string AuthenticationScheme { get; set; } = IdentityConstants.ApplicationScheme;

    public HttpContext Context
    {
      get
      {
        return (this._context ?? this._contextAccessor?.HttpContext) ?? throw new InvalidOperationException("HttpContext must not be null.");
      }
      set => this._context = value;
    }

    public virtual async Task<ClaimsPrincipal> CreateUserPrincipalAsync(TUser user)
    {
      return await this.ClaimsFactory.CreateAsync(user);
    }

    public virtual bool IsSignedIn(ClaimsPrincipal principal)
    {
      ArgumentNullException.ThrowIfNull((object) principal, nameof (principal));
      return principal.Identities != null && principal.Identities.Any<ClaimsIdentity>((Func<ClaimsIdentity, bool>) (i => i.AuthenticationType == this.AuthenticationScheme));
    }

    public virtual async Task<bool> CanSignInAsync(TUser user)
    {
      bool flag1 = this.Options.SignIn.RequireConfirmedEmail;
      if (flag1)
        flag1 = !await this.UserManager.IsEmailConfirmedAsync(user);
      if (flag1)
      {
        this.Logger.LogDebug(EventIds.UserCannotSignInWithoutConfirmedEmail, "User cannot sign in without a confirmed email.");
        return false;
      }
      bool flag2 = this.Options.SignIn.RequireConfirmedPhoneNumber;
      if (flag2)
        flag2 = !await this.UserManager.IsPhoneNumberConfirmedAsync(user);
      if (flag2)
      {
        this.Logger.LogDebug(EventIds.UserCannotSignInWithoutConfirmedPhoneNumber, "User cannot sign in without a confirmed phone number.");
        return false;
      }
      bool flag3 = this.Options.SignIn.RequireConfirmedAccount;
      if (flag3)
        flag3 = !await this._confirmation.IsConfirmedAsync(this.UserManager, user);
      if (!flag3)
        return true;
      this.Logger.LogDebug(EventIds.UserCannotSignInWithoutConfirmedAccount, "User cannot sign in without a confirmed account.");
      return false;
    }

    public virtual async Task RefreshSignInAsync(TUser user)
    {
      AuthenticateResult authenticateResult = await this.Context.AuthenticateAsync(this.AuthenticationScheme);
      IList<Claim> additionalClaims = (IList<Claim>) Array.Empty<Claim>();
      Claim first1 = authenticateResult?.Principal?.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod");
      Claim first2 = authenticateResult?.Principal?.FindFirst("amr");
      if (first1 != null || first2 != null)
      {
        additionalClaims = (IList<Claim>) new List<Claim>();
        if (first1 != null)
          additionalClaims.Add(first1);
        if (first2 != null)
          additionalClaims.Add(first2);
      }
      await this.SignInWithClaimsAsync(user, authenticateResult?.Properties, (IEnumerable<Claim>) additionalClaims);
    }

    public virtual Task SignInAsync(TUser user, bool isPersistent, string? authenticationMethod = null)
    {
      TUser user1 = user;
      AuthenticationProperties authenticationProperties = new AuthenticationProperties();
      authenticationProperties.IsPersistent = isPersistent;
      string authenticationMethod1 = authenticationMethod;
      return this.SignInAsync(user1, authenticationProperties, authenticationMethod1);
    }

    public virtual Task SignInAsync(
      TUser user,
      AuthenticationProperties authenticationProperties,
      string? authenticationMethod = null)
    {
      IList<Claim> additionalClaims = (IList<Claim>) Array.Empty<Claim>();
      if (authenticationMethod != null)
      {
        additionalClaims = (IList<Claim>) new List<Claim>();
        additionalClaims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", authenticationMethod));
      }
      return this.SignInWithClaimsAsync(user, authenticationProperties, (IEnumerable<Claim>) additionalClaims);
    }

    public virtual Task SignInWithClaimsAsync(
      TUser user,
      bool isPersistent,
      IEnumerable<Claim> additionalClaims)
    {
      TUser user1 = user;
      AuthenticationProperties authenticationProperties = new AuthenticationProperties();
      authenticationProperties.IsPersistent = isPersistent;
      IEnumerable<Claim> additionalClaims1 = additionalClaims;
      return this.SignInWithClaimsAsync(user1, authenticationProperties, additionalClaims1);
    }

    public virtual async Task SignInWithClaimsAsync(
      TUser user,
      AuthenticationProperties? authenticationProperties,
      IEnumerable<Claim> additionalClaims)
    {
      ClaimsPrincipal userPrincipal = await this.CreateUserPrincipalAsync(user);
      foreach (Claim additionalClaim in additionalClaims)
        userPrincipal.Identities.First<ClaimsIdentity>().AddClaim(additionalClaim);
      await this.Context.SignInAsync(this.AuthenticationScheme, userPrincipal, authenticationProperties ?? new AuthenticationProperties());
      this.Context.User = userPrincipal;
      userPrincipal = (ClaimsPrincipal) null;
    }

    public virtual async Task SignOutAsync()
    {
      await this.Context.SignOutAsync(this.AuthenticationScheme);
      if (await this._schemes.GetSchemeAsync(IdentityConstants.ExternalScheme) != null)
        await this.Context.SignOutAsync(IdentityConstants.ExternalScheme);
      if (await this._schemes.GetSchemeAsync(IdentityConstants.TwoFactorUserIdScheme) == null)
        return;
      await this.Context.SignOutAsync(IdentityConstants.TwoFactorUserIdScheme);
    }

    public virtual async Task<TUser?> ValidateSecurityStampAsync(ClaimsPrincipal? principal)
    {
      if (principal == null)
        return default (TUser);
      TUser user = await this.UserManager.GetUserAsync(principal);
      if (await this.ValidateSecurityStampAsync(user, principal.FindFirstValue(this.Options.ClaimsIdentity.SecurityStampClaimType)))
        return user;
      this.Logger.LogDebug(EventIds.SecurityStampValidationFailedId4, "Failed to validate a security stamp.");
      return default (TUser);
    }

    public virtual async Task<TUser?> ValidateTwoFactorSecurityStampAsync(ClaimsPrincipal? principal)
    {
      if (principal == null || principal.Identity?.Name == null)
        return default (TUser);
      TUser user = await this.UserManager.FindByIdAsync(principal.Identity.Name);
      if (await this.ValidateSecurityStampAsync(user, principal.FindFirstValue(this.Options.ClaimsIdentity.SecurityStampClaimType)))
        return user;
      this.Logger.LogDebug(EventIds.TwoFactorSecurityStampValidationFailed, "Failed to validate a security stamp.");
      return default (TUser);
    }

    public virtual async Task<bool> ValidateSecurityStampAsync(TUser? user, string? securityStamp)
    {
      bool flag1 = (object) user != null;
      if (flag1)
      {
        bool flag2 = !this.UserManager.SupportsUserSecurityStamp;
        if (!flag2)
        {
          string str = securityStamp;
          flag2 = str == await this.UserManager.GetSecurityStampAsync(user);
          str = (string) null;
        }
        flag1 = flag2;
      }
      return flag1;
    }

    public virtual async Task<SignInResult> PasswordSignInAsync(
      TUser user,
      string password,
      bool isPersistent,
      bool lockoutOnFailure)
    {
      ArgumentNullException.ThrowIfNull((object) user, nameof (user));
      SignInResult signInResult1 = await this.CheckPasswordSignInAsync(user, password, lockoutOnFailure);
      SignInResult signInResult2;
      if (signInResult1.Succeeded)
        signInResult2 = await this.SignInOrTwoFactorAsync(user, isPersistent);
      else
        signInResult2 = signInResult1;
      return signInResult2;
    }

    public virtual async Task<SignInResult> PasswordSignInAsync(
      string userName,
      string password,
      bool isPersistent,
      bool lockoutOnFailure)
    {
      TUser byNameAsync = await this.UserManager.FindByNameAsync(userName);
      return (object) byNameAsync == null ? SignInResult.Failed : await this.PasswordSignInAsync(byNameAsync, password, isPersistent, lockoutOnFailure);
    }

    public virtual async Task<SignInResult> CheckPasswordSignInAsync(
      TUser user,
      string password,
      bool lockoutOnFailure)
    {
      ArgumentNullException.ThrowIfNull((object) user, nameof (user));
      SignInResult signInResult = await this.PreSignInCheck(user);
      if (signInResult != null)
        return signInResult;
      if (await this.UserManager.CheckPasswordAsync(user, password))
      {
        bool isEnabled;
        bool flag1 = AppContext.TryGetSwitch("Microsoft.AspNetCore.Identity.CheckPasswordSignInAlwaysResetLockoutOnSuccess", out isEnabled) & isEnabled;
        if (!flag1)
          flag1 = !await this.IsTwoFactorEnabledAsync(user);
        bool flag2 = flag1;
        if (!flag2)
          flag2 = await this.IsTwoFactorClientRememberedAsync(user);
        if (flag2)
        {
          if (!(await this.ResetLockoutWithResult(user)).Succeeded)
            return SignInResult.Failed;
        }
        return SignInResult.Success;
      }
      this.Logger.LogDebug(EventIds.InvalidPassword, "User failed to provide the correct password.");
      if (this.UserManager.SupportsUserLockout & lockoutOnFailure)
      {
        if (!(await this.UserManager.AccessFailedAsync(user) ?? IdentityResult.Success).Succeeded)
          return SignInResult.Failed;
        if (await this.UserManager.IsLockedOutAsync(user))
          return await this.LockedOut(user);
      }
      return SignInResult.Failed;
    }

    public virtual async Task<bool> IsTwoFactorClientRememberedAsync(TUser user)
    {
      if (await this._schemes.GetSchemeAsync(IdentityConstants.TwoFactorRememberMeScheme) == null)
        return false;
      string userId = await this.UserManager.GetUserIdAsync(user);
      AuthenticateResult authenticateResult = await this.Context.AuthenticateAsync(IdentityConstants.TwoFactorRememberMeScheme);
      return authenticateResult?.Principal != null && authenticateResult.Principal.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name") == userId;
    }

    public virtual async Task RememberTwoFactorClientAsync(TUser user)
    {
      ClaimsPrincipal principal = await this.StoreRememberClient(user);
      await this.Context.SignInAsync(IdentityConstants.TwoFactorRememberMeScheme, principal, new AuthenticationProperties()
      {
        IsPersistent = true
      });
    }

    public virtual Task ForgetTwoFactorClientAsync()
    {
      return this.Context.SignOutAsync(IdentityConstants.TwoFactorRememberMeScheme);
    }

    public virtual async Task<SignInResult> TwoFactorRecoveryCodeSignInAsync(string recoveryCode)
    {
      SignInManager<TUser>.TwoFactorAuthenticationInfo twoFactorInfo = await this.RetrieveTwoFactorInfoAsync();
      if (twoFactorInfo == null)
        return SignInResult.Failed;
      return (await this.UserManager.RedeemTwoFactorRecoveryCodeAsync(twoFactorInfo.User, recoveryCode)).Succeeded ? await this.DoTwoFactorSignInAsync(twoFactorInfo.User, twoFactorInfo, false, false) : SignInResult.Failed;
    }

    #nullable disable
    private async Task<SignInResult> DoTwoFactorSignInAsync(
      TUser user,
      SignInManager<TUser>.TwoFactorAuthenticationInfo twoFactorInfo,
      bool isPersistent,
      bool rememberClient)
    {
      if (!(await this.ResetLockoutWithResult(user)).Succeeded)
        return SignInResult.Failed;
      List<Claim> claims = new List<Claim>();
      claims.Add(new Claim("amr", "mfa"));
      if (twoFactorInfo.LoginProvider != null)
        claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", twoFactorInfo.LoginProvider));
      if (await this._schemes.GetSchemeAsync(IdentityConstants.ExternalScheme) != null)
        await this.Context.SignOutAsync(IdentityConstants.ExternalScheme);
      if (await this._schemes.GetSchemeAsync(IdentityConstants.TwoFactorUserIdScheme) != null)
      {
        await this.Context.SignOutAsync(IdentityConstants.TwoFactorUserIdScheme);
        if (rememberClient)
          await this.RememberTwoFactorClientAsync(user);
      }
      await this.SignInWithClaimsAsync(user, isPersistent, (IEnumerable<Claim>) claims);
      return SignInResult.Success;
    }

    #nullable enable
    public virtual async Task<SignInResult> TwoFactorAuthenticatorSignInAsync(
      string code,
      bool isPersistent,
      bool rememberClient)
    {
      SignInManager<TUser>.TwoFactorAuthenticationInfo twoFactorInfo = await this.RetrieveTwoFactorInfoAsync();
      if (twoFactorInfo == null)
        return SignInResult.Failed;
      TUser user = twoFactorInfo.User;
      SignInResult signInResult = await this.PreSignInCheck(user);
      if (signInResult != null)
        return signInResult;
      if (await this.UserManager.VerifyTwoFactorTokenAsync(user, this.Options.Tokens.AuthenticatorTokenProvider, code))
        return await this.DoTwoFactorSignInAsync(user, twoFactorInfo, isPersistent, rememberClient);
      if (this.UserManager.SupportsUserLockout)
      {
        if (!(await this.UserManager.AccessFailedAsync(user) ?? IdentityResult.Success).Succeeded)
          return SignInResult.Failed;
        if (await this.UserManager.IsLockedOutAsync(user))
          return await this.LockedOut(user);
      }
      return SignInResult.Failed;
    }

    public virtual async Task<SignInResult> TwoFactorSignInAsync(
      string provider,
      string code,
      bool isPersistent,
      bool rememberClient)
    {
      SignInManager<TUser>.TwoFactorAuthenticationInfo twoFactorInfo = await this.RetrieveTwoFactorInfoAsync();
      if (twoFactorInfo == null)
        return SignInResult.Failed;
      TUser user = twoFactorInfo.User;
      SignInResult signInResult = await this.PreSignInCheck(user);
      if (signInResult != null)
        return signInResult;
      if (await this.UserManager.VerifyTwoFactorTokenAsync(user, provider, code))
        return await this.DoTwoFactorSignInAsync(user, twoFactorInfo, isPersistent, rememberClient);
      if (this.UserManager.SupportsUserLockout)
      {
        if (!(await this.UserManager.AccessFailedAsync(user) ?? IdentityResult.Success).Succeeded)
          return SignInResult.Failed;
        if (await this.UserManager.IsLockedOutAsync(user))
          return await this.LockedOut(user);
      }
      return SignInResult.Failed;
    }

    public virtual async Task<TUser?> GetTwoFactorAuthenticationUserAsync()
    {
      SignInManager<TUser>.TwoFactorAuthenticationInfo authenticationInfo = await this.RetrieveTwoFactorInfoAsync();
      return authenticationInfo != null ? authenticationInfo.User : default (TUser);
    }

    public virtual Task<SignInResult> ExternalLoginSignInAsync(
      string loginProvider,
      string providerKey,
      bool isPersistent)
    {
      return this.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent, false);
    }

    public virtual async Task<SignInResult> ExternalLoginSignInAsync(
      string loginProvider,
      string providerKey,
      bool isPersistent,
      bool bypassTwoFactor)
    {
      TUser user = await this.UserManager.FindByLoginAsync(loginProvider, providerKey);
      if ((object) user == null)
        return SignInResult.Failed;
      SignInResult signInResult = await this.PreSignInCheck(user);
      return signInResult != null ? signInResult : await this.SignInOrTwoFactorAsync(user, isPersistent, loginProvider, bypassTwoFactor);
    }

    public virtual async Task<IEnumerable<Microsoft.AspNetCore.Authentication.AuthenticationScheme>> GetExternalAuthenticationSchemesAsync()
    {
      return (await this._schemes.GetAllSchemesAsync()).Where<Microsoft.AspNetCore.Authentication.AuthenticationScheme>((Func<Microsoft.AspNetCore.Authentication.AuthenticationScheme, bool>) (s => !string.IsNullOrEmpty(s.DisplayName)));
    }

    public virtual async Task<ExternalLoginInfo?> GetExternalLoginInfoAsync(string? expectedXsrf = null)
    {
      AuthenticateResult auth = await this.Context.AuthenticateAsync(IdentityConstants.ExternalScheme);
      IDictionary<string, string> items = auth?.Properties?.Items;
      string str;
      string provider;
      if (auth?.Principal == null || items == null || !items.TryGetValue("LoginProvider", out provider) || expectedXsrf != null && (!items.TryGetValue("XsrfId", out str) || str != expectedXsrf))
        return (ExternalLoginInfo) null;
      string providerKey = auth.Principal.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier") ?? auth.Principal.FindFirstValue("sub");
      if (providerKey == null || provider == null)
        return (ExternalLoginInfo) null;
      ExternalLoginInfo externalLoginInfoAsync = new ExternalLoginInfo(auth.Principal, provider, providerKey, (await this.GetExternalAuthenticationSchemesAsync()).FirstOrDefault<Microsoft.AspNetCore.Authentication.AuthenticationScheme>((Func<Microsoft.AspNetCore.Authentication.AuthenticationScheme, bool>) (p => p.Name == provider))?.DisplayName ?? provider);
      AuthenticationProperties properties = auth.Properties;
      externalLoginInfoAsync.AuthenticationTokens = properties != null ? properties.GetTokens() : (IEnumerable<AuthenticationToken>) null;
      externalLoginInfoAsync.AuthenticationProperties = auth.Properties;
      return externalLoginInfoAsync;
    }

    public virtual async Task<IdentityResult> UpdateExternalAuthenticationTokensAsync(
      ExternalLoginInfo externalLogin)
    {
      ArgumentNullException.ThrowIfNull((object) externalLogin, nameof (externalLogin));
      if (externalLogin.AuthenticationTokens != null && externalLogin.AuthenticationTokens.Any<AuthenticationToken>())
      {
        TUser user = await this.UserManager.FindByLoginAsync(externalLogin.LoginProvider, externalLogin.ProviderKey);
        if ((object) user == null)
          return IdentityResult.Failed();
        foreach (AuthenticationToken authenticationToken in externalLogin.AuthenticationTokens)
        {
          IdentityResult identityResult = await this.UserManager.SetAuthenticationTokenAsync(user, externalLogin.LoginProvider, authenticationToken.Name, authenticationToken.Value);
          if (!identityResult.Succeeded)
            return identityResult;
        }
        user = default (TUser);
      }
      return IdentityResult.Success;
    }

    public virtual AuthenticationProperties ConfigureExternalAuthenticationProperties(
      string? provider,
      [StringSyntax("Uri")] string? redirectUrl,
      string? userId = null)
    {
      AuthenticationProperties authenticationProperties = new AuthenticationProperties()
      {
        RedirectUri = redirectUrl
      };
      authenticationProperties.Items["LoginProvider"] = provider;
      if (userId != null)
        authenticationProperties.Items["XsrfId"] = userId;
      return authenticationProperties;
    }

    internal static ClaimsPrincipal StoreTwoFactorInfo(string userId, string? loginProvider)
    {
      ClaimsIdentity claimsIdentity = new ClaimsIdentity(IdentityConstants.TwoFactorUserIdScheme);
      claimsIdentity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", userId));
      if (loginProvider != null)
        claimsIdentity.AddClaim(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", loginProvider));
      return new ClaimsPrincipal((IIdentity) claimsIdentity);
    }

    internal async Task<ClaimsPrincipal> StoreRememberClient(TUser user)
    {
      string userIdAsync = await this.UserManager.GetUserIdAsync(user);
      ClaimsIdentity rememberBrowserIdentity = new ClaimsIdentity(IdentityConstants.TwoFactorRememberMeScheme);
      rememberBrowserIdentity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", userIdAsync));
      if (this.UserManager.SupportsUserSecurityStamp)
      {
        string securityStampAsync = await this.UserManager.GetSecurityStampAsync(user);
        rememberBrowserIdentity.AddClaim(new Claim(this.Options.ClaimsIdentity.SecurityStampClaimType, securityStampAsync));
      }
      ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal((IIdentity) rememberBrowserIdentity);
      rememberBrowserIdentity = (ClaimsIdentity) null;
      return claimsPrincipal;
    }

    public virtual async Task<bool> IsTwoFactorEnabledAsync(TUser user)
    {
      bool flag1 = this.UserManager.SupportsUserTwoFactor;
      if (flag1)
        flag1 = await this.UserManager.GetTwoFactorEnabledAsync(user);
      bool flag2 = flag1;
      if (flag2)
        flag2 = (await this.UserManager.GetValidTwoFactorProvidersAsync(user)).Count > 0;
      return flag2;
    }

    protected virtual async Task<SignInResult> SignInOrTwoFactorAsync(
      TUser user,
      bool isPersistent,
      string? loginProvider = null,
      bool bypassTwoFactor = false)
    {
      SignInManager<TUser> signInManager = this;
      bool flag = !bypassTwoFactor;
      if (flag)
        flag = await signInManager.IsTwoFactorEnabledAsync(user);
      if (flag)
      {
        if (!await signInManager.IsTwoFactorClientRememberedAsync(user))
        {
          signInManager._twoFactorInfo = new SignInManager<TUser>.TwoFactorAuthenticationInfo()
          {
            User = user,
            LoginProvider = loginProvider
          };
          if (await signInManager._schemes.GetSchemeAsync(IdentityConstants.TwoFactorUserIdScheme) != null)
          {
            string userIdAsync = await signInManager.UserManager.GetUserIdAsync(user);
            await signInManager.Context.SignInAsync(IdentityConstants.TwoFactorUserIdScheme, SignInManager<TUser>.StoreTwoFactorInfo(userIdAsync, loginProvider));
          }
          return SignInResult.TwoFactorRequired;
        }
      }
      if (loginProvider != null)
        await signInManager.Context.SignOutAsync(IdentityConstants.ExternalScheme);
      if (loginProvider == null)
        await signInManager.SignInWithClaimsAsync(user, (isPersistent ? 1 : 0) != 0, (IEnumerable<Claim>) new Claim[1]
        {
          new Claim("amr", "pwd")
        });
      else
        await signInManager.SignInAsync(user, isPersistent, loginProvider);
      return SignInResult.Success;
    }

    #nullable disable
    private async Task<SignInManager<TUser>.TwoFactorAuthenticationInfo> RetrieveTwoFactorInfoAsync()
    {
      if (this._twoFactorInfo != null)
        return this._twoFactorInfo;
      AuthenticateResult result = await this.Context.AuthenticateAsync(IdentityConstants.TwoFactorUserIdScheme);
      if (result?.Principal == null)
        return (SignInManager<TUser>.TwoFactorAuthenticationInfo) null;
      string firstValue = result.Principal.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
      if (firstValue == null)
        return (SignInManager<TUser>.TwoFactorAuthenticationInfo) null;
      TUser byIdAsync = await this.UserManager.FindByIdAsync(firstValue);
      if ((object) byIdAsync == null)
        return (SignInManager<TUser>.TwoFactorAuthenticationInfo) null;
      return new SignInManager<TUser>.TwoFactorAuthenticationInfo()
      {
        User = byIdAsync,
        LoginProvider = result.Principal.FindFirstValue("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod")
      };
    }

    #nullable enable
    protected virtual async Task<bool> IsLockedOut(TUser user)
    {
      bool flag = this.UserManager.SupportsUserLockout;
      if (flag)
        flag = await this.UserManager.IsLockedOutAsync(user);
      return flag;
    }

    protected virtual Task<SignInResult> LockedOut(TUser user)
    {
      this.Logger.LogDebug(EventIds.UserLockedOut, "User is currently locked out.");
      return Task.FromResult<SignInResult>(SignInResult.LockedOut);
    }

    protected virtual async Task<SignInResult?> PreSignInCheck(TUser user)
    {
      if (!await this.CanSignInAsync(user))
        return SignInResult.NotAllowed;
      return await this.IsLockedOut(user) ? await this.LockedOut(user) : (SignInResult) null;
    }

    protected virtual async Task ResetLockout(TUser user)
    {
      if (!this.UserManager.SupportsUserLockout)
        return;
      IdentityResult result = await this.UserManager.ResetAccessFailedCountAsync(user) ?? IdentityResult.Success;
      if (!result.Succeeded)
        throw new SignInManager<TUser>.IdentityResultException(result);
    }

    #nullable disable
    private async Task<IdentityResult> ResetLockoutWithResult(TUser user)
    {
      SignInManager<TUser> signInManager = this;
      if (signInManager.GetType() == typeof (SignInManager<TUser>))
        return !signInManager.UserManager.SupportsUserLockout ? IdentityResult.Success : await signInManager.UserManager.ResetAccessFailedCountAsync(user) ?? IdentityResult.Success;
      try
      {
        Task task1 = signInManager.ResetLockout(user);
        if (task1 is Task<IdentityResult> task2)
          return await task2 ?? IdentityResult.Success;
        await task1;
        return IdentityResult.Success;
      }
      catch (SignInManager<TUser>.IdentityResultException ex)
      {
        return ex.IdentityResult;
      }
    }

    private sealed class IdentityResultException : Exception
    {
      internal IdentityResultException(IdentityResult result) => this.IdentityResult = result;

      internal IdentityResult IdentityResult { get; set; }

      public override string Message
      {
        get
        {
          StringBuilder stringBuilder = new StringBuilder("ResetLockout failed.");
          foreach (IdentityError error in this.IdentityResult.Errors)
          {
            stringBuilder.AppendLine();
            stringBuilder.Append(error.Code);
            stringBuilder.Append(": ");
            stringBuilder.Append(error.Description);
          }
          return stringBuilder.ToString();
        }
      }
    }

    #nullable enable
    [RequiredMember]
    internal sealed class TwoFactorAuthenticationInfo
    {
      [RequiredMember]
      public TUser User { get; init; }

      public string? LoginProvider { get; init; }

      [Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
      [CompilerFeatureRequired("RequiredMembers")]
      public TwoFactorAuthenticationInfo()
      {
      }
    }
  }
}
