using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VisaCheckOut.AuthorizeServer.Infrastructure;
using VisaCheckOut.Infrastructure;
using VisaCheckOut.Models;
using VisaCheckOut.Models.VisaCheckoutOutputs.UserOut;
using VisaCheckOut.BL;
using VisaCheckOut.Models.VisaCheckoutInputs.UserManagementInp;
using System.Configuration;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;

namespace VisaCheckOut.Controllers
{
    
    [RoutePrefix("api/account")]

    public class AccountController : BaseApiController
    {
        VisaCheckOutEntities ve = new VisaCheckOutEntities();

       [ClaimsAuthorization(ClaimType = "Createuser", ClaimValue = "1")]
        [Route("createuser")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateUser(CreateUserBindingModel createUserModel)
        {
            VisaCheckOut.Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.UserOut.UserDTO> dto = new DTO<Models.VisaCheckoutOutputs.UserOut.UserDTO>();
            dto.objname = "createuser";
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            if (string.IsNullOrEmpty(createUserModel.FirstName) || string.IsNullOrEmpty(createUserModel.LastName) || string.IsNullOrEmpty(createUserModel.Username) || string.IsNullOrEmpty(createUserModel.Email))
            {
                dto.status = new VisaCheckOut.Models.Status(5);
                return Ok(dto);
            }
            if (string.IsNullOrEmpty(createUserModel.Userid) && string.IsNullOrEmpty(createUserModel.Password))
            {
                dto.status = new VisaCheckOut.Models.Status(5);
                return Ok(dto);
            }
            else
            {
                if (!string.IsNullOrEmpty(createUserModel.Userid))
                {
                    var euser = await this.AppUserManager.FindByIdAsync(createUserModel.Userid);
                    if (euser == null)
                    {
                        dto.status = new VisaCheckOut.Models.Status(7);
                        dto.status.statusdescription = "User" + " " + dto.status.statusdescription;
                        return Ok(dto);
                    }
                    if (euser.Email != createUserModel.Email)
                    {
                        var users = this.AppUserManager.Users.Where(a => a.Email.ToLower() == createUserModel.Email.ToLower()).FirstOrDefault();
                        if (users != null)
                        {
                            dto.status = new VisaCheckOut.Models.Status(6);
                            dto.status.statusdescription = "Email ID" + " " + dto.status.statusdescription;
                            return Ok(dto);
                        }
                    }
                    if (euser.UserName != createUserModel.Username)
                    {
                        var users = this.AppUserManager.Users.Where(a => a.UserName.ToLower() == createUserModel.Username.ToLower()).FirstOrDefault();
                        if (users != null)
                        {
                            dto.status = new VisaCheckOut.Models.Status(6);
                            dto.status.statusdescription = "User Name" + " " + dto.status.statusdescription;
                            return Ok(dto);
                        }
                    }
                }
                else
                {
                    var users = this.AppUserManager.Users.Where(a => a.UserName.ToLower() == createUserModel.Username.ToLower()).FirstOrDefault();
                    if (users != null)
                    {
                        dto.status = new VisaCheckOut.Models.Status(6);
                        dto.status.statusdescription = "User Name" + " " + dto.status.statusdescription;
                        return Ok(dto);
                    }
                    if (!string.IsNullOrEmpty(createUserModel.Email))
                    {
                        var euserss = this.AppUserManager.Users.Where(a => a.Email.ToLower() == createUserModel.Email.ToLower()).FirstOrDefault();
                        if (euserss != null)
                        {
                            if (euserss.Email.ToLower() == createUserModel.Email.ToLower())
                            {
                                dto.status = new VisaCheckOut.Models.Status(6);
                                dto.status.statusdescription = "Email ID" + " " + dto.status.statusdescription;
                                return Ok(dto);
                            }
                        }

                    }
                }
            }
            if (!string.IsNullOrEmpty(createUserModel.Userid))
            {
                var users = await this.AppUserManager.FindByIdAsync(createUserModel.Userid);
                if (users != null)
                {
                    users.UserName = createUserModel.Username;
                    users.Email = createUserModel.Email;
                    users.FirstName = createUserModel.FirstName;
                    users.LastName = createUserModel.LastName;
                    users.PhoneNumber = createUserModel.PhoneNumber;
                    users.Level = 3;
                    users.EmailConfirmed = true;
                    users.JoinDate = users.JoinDate;
                    users.CreatedDate = users.CreatedDate;
                    users.UpdatedDate = DateTime.Now;
                }
                IdentityResult addUserResul = await this.AppUserManager.UpdateAsync(users);
                if (!addUserResul.Succeeded)
                {
                    return GetErrorResult(addUserResul);
                }
                dto.status = new Status();
                dto.status.statuscode = "200";
                dto.status.statusdescription = "OK";
                return Ok(dto);
            }
            else
            {
                var user = new ApplicationUser()
                {
                    UserName = createUserModel.Username,
                    Email = createUserModel.Email,
                    FirstName = createUserModel.FirstName,
                    LastName = createUserModel.LastName,
                    PhoneNumber = createUserModel.PhoneNumber,
                    Level = 3,
                    EmailConfirmed = true,
                    Status = true,
                    JoinDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                IdentityResult addUserResult = await this.AppUserManager.CreateAsync(user, createUserModel.Password);
                if (!addUserResult.Succeeded)
                {
                    return GetErrorResult(addUserResult);
                }
                // string code = await this.AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);

                //   var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = code }));

                //   await this.AppUserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                //  Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));
                dto.status = new Status();
                dto.status.statuscode = "200";
                dto.status.statusdescription = "OK";
                return Ok(dto);
            }
        }

        [ClaimsAuthorization(ClaimType = "AssignRolesToUser", ClaimValue = "1")]
        [Route("user/{id:guid}/roles")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] string id, [FromBody] string[] rolesToAssign)
        {
            //string message = "";
            var appUser = await this.AppUserManager.FindByIdAsync(id);
            //getting roles for user 
            //check comng role name id exist or not 
            var userrole = this.AppRoleManager.FindByName(rolesToAssign[0]);
            string message = "";
            if (userrole!=null)
            {
                
               foreach (var roles in appUser.Roles)
                {
                    if(roles.RoleId==userrole.Id)
                    {
                        message = "Role Already Assigned To User";
                        return Ok(message);
                    }
                }                
            }
            if (appUser == null)
            {
                return NotFound();
            }

            var currentRoles = await this.AppUserManager.GetRolesAsync(appUser.Id);

            var rolesNotExists = rolesToAssign.Except(this.AppRoleManager.Roles.Select(x => x.Name)).ToArray();

            if (rolesNotExists.Count() > 0)
            {
                ModelState.AddModelError("", string.Format("Roles '{0}' does not exixts in the system", string.Join(",", rolesNotExists)));
                return BadRequest(ModelState);
            }

            IdentityResult removeResult = await this.AppUserManager.RemoveFromRolesAsync(appUser.Id, currentRoles.ToArray());

            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return BadRequest(ModelState);
            }

            IdentityResult addResult = await this.AppUserManager.AddToRolesAsync(appUser.Id, rolesToAssign);

            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }

            return Ok(message);
        }


        [ClaimsAuthorization(ClaimType = "GetUsers", ClaimValue = "1")]
        [Route("users")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUsers()
        {

            //iRepository<ApplicationUserManager> repuser = new Repository<ApplicationUserManager>();
            List<UserDTO> listusers = new List<UserDTO>();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var getusersdata = this.AppUserManager.Users.ToList();
            if (getusersdata != null)
            {
                foreach (var item in getusersdata)
                {
                    UserDTO model = new UserDTO();
                    model.Id = item.Id;
                    model.fullName = item.FirstName + " " + item.LastName;
                    model.FirstName = item.FirstName;
                    model.LastName = item.LastName;
                    model.Phonenumber = item.PhoneNumber;
                    model.Email = item.Email;
                    model.Username = item.UserName;
                    model.rolename = "";
                    model.CreatedDate = item.CreatedDate;
                    model.Status = item.Status.ToString();
                    model.Updateddate = Convert.ToDateTime(item.UpdatedDate);
                    listusers.Add(model);
                }
            }
            return Ok(listusers);

        }


        [ClaimsAuthorization(ClaimType = "DeleteUser", ClaimValue = "1")]
        [HttpGet]
        [Route("DeleteUser/{id:guid}")]
        public async Task<IHttpActionResult> DeleteUser([FromUri] string id)
        {

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            string status = "true";
            // var getblruser = repblrusers.GetAll().Where(a => a.UserID == id).FirstOrDefault();

            try
            {
                if (appUser != null)
                {
                    if (appUser.Status == true)
                    {
                        appUser.Status = false;
                        appUser.UpdatedDate = DateTime.Now;

                        status = "false";
                    }
                    else
                    {
                        appUser.Status = true;
                        appUser.UpdatedDate = DateTime.Now;
                        status = "true";
                    }

                    IdentityResult result = await this.AppUserManager.UpdateAsync(appUser);
                    if (!result.Succeeded)
                    {
                        return GetErrorResult(result);
                    }
                }
            }

            catch (Exception ex)
            {
                return NotFound();
            }
            return Ok(status);

        }


        [ClaimsAuthorization(ClaimType = "GetAllRoles", ClaimValue = "1")]
        [Route("getAllRoles")]
        public IHttpActionResult GetAllRoles()
        {
            List<RolesDTO> dto = new List<RolesDTO>();
            var roles = this.AppRoleManager.Roles.ToList();
            try
            {
                if (roles != null)
                {
                    dto = roles.Select(a => new RolesDTO
                    {
                        roleid = a.Id,
                        rolename = a.Name
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return Ok(dto);
        }

        [ClaimsAuthorization(ClaimType = "Createrole", ClaimValue = "1")]
        [Route("Createrole")]
        [HttpPut]
        public async Task<IHttpActionResult> Create(CreateRoleBindingModel model)
        {
            VisaCheckOut.Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.UserOut.RolesDTO> dto = new VisaCheckOut.Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.UserOut.RolesDTO>();
            var role = new IdentityRole { Name = model.Name };
            var result = await this.AppRoleManager.CreateAsync(role);
            string useridid = System.Web.HttpContext.Current.User.Identity.GetUserId();
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            dto.status = new VisaCheckOut.Models.Status(0);
            return Ok(dto);
        }


        public string CheckRole()
        {
            string msg = "";
            string uid = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var userroles = this.AppUserManager.FindById(uid).Roles.ToList();
            if (userroles != null)
            {
                foreach (var i in userroles)
                {

                    var currentRoles = this.AppRoleManager.FindById(i.RoleId).Name;

                    bool isBilleradmin = currentRoles.ToLower() == "billeradmin";
                    if (isBilleradmin)
                    {
                        msg = "Biller";
                    }
                    else
                    {
                        msg = "Og";
                    }
                }
            }
            return msg;
        }

        [ClaimsAuthorization(ClaimType = "GetUserRoles", ClaimValue = "1")]
        [Route("getUserRoles")]
        [HttpGet]
        public IHttpActionResult GetUserRoles()
        {
            List<UserRolesDTO> dto = new List<UserRolesDTO>();
            var roles = this.AppRoleManager.Roles.ToList();
            var users = this.AppUserManager.Users.ToList();
            try
            {
                if(users!=null)
                {
                    foreach (var usr in users)
                    {
                        var usess = this.AppUserManager.FindById(usr.Id).Roles;
                        foreach (var ssdd in usess)
                        {
                            var rolename = this.AppRoleManager.FindById(ssdd.RoleId);
                            UserRolesDTO res = new UserRolesDTO();
                            res.Username = usr.UserName;
                            res.RoleName = rolename.Name;
                            dto.Add(res);
                        }

                    }
                }

            }
            catch (Exception ex)
            {

            }
            return Ok(dto);
        }


        //Assign Merchant to User

        //[ClaimsAuthorization(ClaimType = "CreateMerchantUser", ClaimValue = "1")]
        [Route("CreateMerchantUser")]
        [HttpPut]
        public async Task<IHttpActionResult> CreateMerchantUser(Models.VisaCheckoutInputs.UserManagementInp.CreateMerchantUserInp inp)
        {
            string mid = inp.MerchantID;
            VisaCheckOut.Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.UserOut.MerchantUsersDTO> dto = new DTO<MerchantUsersDTO>();
            var merchantname = ve.Merchants.Where(a => a.MerchantId == mid).FirstOrDefault();
            var getusers = this.AppUserManager.FindById(inp.UserID);
            try
            {
                if (merchantname != null)
                {
                    inp.MerchantName = merchantname.CompanyPrimaryLegalName;
                }
                if (getusers != null)
                {
                    inp.UserName = getusers.UserName;
                }
                dto= SaveMerchantUser(inp);
            }
            catch (HttpRequestException e)
            {

            }

            return Ok(dto);
        }

        private VisaCheckOut.Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.UserOut.MerchantUsersDTO> SaveMerchantUser(CreateMerchantUserInp inp)
        {

            VisaCheckOut.Models.DTO<VisaCheckOut.Models.VisaCheckoutOutputs.UserOut.MerchantUsersDTO> dto = new DTO<MerchantUsersDTO>();
            string useriD = System.Web.HttpContext.Current.User.Identity.GetUserId();
            dto.objname = "CreateMerchantUser";
            string merchantid = "";
            string merchantname = "";
            int merchantuserid = 0;
            string username = "";

            try
            {
                if (string.IsNullOrEmpty(inp.MerchantID) || string.IsNullOrEmpty(inp.UserID))
                {
                    dto.status = new Models.Status(5);
                    return dto;
                }
                merchantid = inp.MerchantID;
                username = inp.UserName;
                var getusername = ve.MerchantUsers.Where(a => a.UserID == inp.UserID).FirstOrDefault();
                if (getusername != null)
                {
                    username = getusername.UserName;
                }
                if (!string.IsNullOrEmpty(inp.MerchantUserID))
                {
                     merchantuserid = Convert.ToInt32(inp.MerchantUserID);


                    var getchkusr = ve.MerchantUsers.Where(a => a.UserID == inp.UserID && a.ID != merchantuserid).FirstOrDefault();

                    if (getchkusr == null)
                    {
                        var getbllrchkusr = ve.MerchantUsers.Where(a => a.UserID == inp.UserID && a.MerchantID ==merchantid && a.ID != merchantuserid).FirstOrDefault();
                        if (getbllrchkusr != null)
                        {
                            dto.status = new Models.Status(6);
                            dto.status.statusdescription = "Merchant with same User" + " " + dto.status.statusdescription;
                            return dto;
                        }

                    }
                    else
                    {
                        var getbllrchkusr = ve.MerchantUsers.Where(a => a.ID != merchantuserid && a.MerchantID == merchantid && a.UserID == inp.UserID).FirstOrDefault();
                        if (getbllrchkusr != null)
                        {
                            dto.status = new Models.Status(6);
                            dto.status.statusdescription = "Merchant with same User" + " " + dto.status.statusdescription;
                            return dto;
                        }
                        else
                        {
                            var getbllrchkusr1 = ve.MerchantUsers.Where(a => a.ID != merchantuserid && a.UserID == inp.UserID && a.MerchantID != merchantid).FirstOrDefault();
                            if (getbllrchkusr1 != null)
                            {
                                dto.status = new Models.Status(6);
                                dto.status.statusdescription = "User was already assigned to another Merchant";
                                return dto;
                            }
                            else
                            {
                                dto.status = new Models.Status(6);
                                dto.status.statusdescription = "User was already assigned to another Merchant";
                                return dto;
                            }

                        }
                    }



                }
                else
                {

                    var getchkusr = ve.MerchantUsers.Where(a => a.UserID == inp.UserID).FirstOrDefault();
                    if (getchkusr == null)
                    {
                        var getbllrchkusr = ve.MerchantUsers.Where(a => a.UserID == inp.UserID && a.MerchantID == merchantid).FirstOrDefault();
                        if (getbllrchkusr != null)
                        {
                            dto.status = new Models.Status(6);
                            dto.status.statusdescription = "Merchant with same User" + " " + dto.status.statusdescription;
                            return dto;
                        }

                    }
                    else
                    {
                        var getbllrchkusr = ve.MerchantUsers.Where(a => a.MerchantID == merchantid && a.UserID == inp.UserID).FirstOrDefault();
                        if (getbllrchkusr != null)
                        {
                            dto.status = new Models.Status(6);
                            dto.status.statusdescription = "Merchant with same User" + " " + dto.status.statusdescription;
                            return dto;
                        }
                        else
                        {
                            var getbllrchkusr1 = ve.MerchantUsers.Where(a => a.MerchantID != merchantid && a.UserID == inp.UserID).FirstOrDefault();
                            if (getbllrchkusr1 != null)
                            {
                                dto.status = new Models.Status(6);
                                dto.status.statusdescription = "User was already assigned to another Merchant";
                                return dto;
                            }
                            else
                            {
                                dto.status = new Models.Status(6);
                                dto.status.statusdescription = "User was already assigned to another Merchant";
                                return dto;
                            }
                        }


                    }

                }

                if (!string.IsNullOrEmpty(inp.MerchantUserID))
                {

                    merchantid =inp.MerchantID;
                    var getmerchantuser = ve.MerchantUsers.Where(a => a.ID == merchantuserid).FirstOrDefault();
                    if (getmerchantuser != null)
                    {
                        getmerchantuser.MerchantID = merchantid;
                        getmerchantuser.UserID = inp.UserID;
                        getmerchantuser.MerchantName = inp.MerchantName;
                        getmerchantuser.UserName = username;
                        getmerchantuser.UpdatedDate = DateTime.Now;
                        getmerchantuser.UpdatedBy = useriD;
                        dto.status = new Models.Status();
                        dto.status.statuscode = "200";
                        dto.status.statusdescription = "OK";
                    }

                }
                else
                {
                    var getmerchanname = ve.Merchants.Where(a => a.MerchantId == merchantid).FirstOrDefault();
                    merchantname= getmerchanname.CompanyPrimaryLegalName;
                    if (getmerchanname!=null)
                    {
                        MerchantUser muser = new MerchantUser();
                        muser.MerchantID = merchantid;
                        muser.MerchantName =merchantname;
                        muser.UserID = inp.UserID;
                        muser.UserName = username;
                        muser.Status = true;
                        muser.CreatedDate = DateTime.Now;
                        muser.UpdatedDate = DateTime.Now;
                        ve.MerchantUsers.Add(muser);
                        ve.SaveChanges();
                        dto.status = new Models.Status();
                        dto.status.statuscode = "200";
                        dto.status.statusdescription = "OK";

                    }
                    
                }

            }
            catch (Exception ex)
            {
                Common.TraceLog.WriteToLog(ex.Message.ToString() + "Trace = " + ex.StackTrace.ToString(), ex);
            }
            dto.status = new Models.Status(0);
            return dto;
        }

        //[ClaimsAuthorization(ClaimType = "GetMerchantUsers", ClaimValue = "1")]
        [Route("GetMerchantUsers")]
        [HttpGet]
        public IHttpActionResult GetMerchantUsers()
        {           
            List<MerchantUsersDTO> res = new List<MerchantUsersDTO>();
            var merchantusers = ve.MerchantUsers.ToList().OrderByDescending(a=>a.ID);
            try
            {
                if (merchantusers != null)
                {
                    res = merchantusers.OrderByDescending(a => a.CreatedDate).ToList().Select(a =>
                     new MerchantUsersDTO
                     {
                         merchantId = a.MerchantID,
                         merchantname=a.MerchantName,
                         userId=a.UserID,
                         username=a.UserName,
                         createdDate=Convert.ToDateTime(a.CreatedDate),
                         status=a.Status.ToString(),
                         merchantuserid=a.ID.ToString()
                     }).ToList();
                }

            }
            catch (Exception ex)
            {

            }
            return Ok(res);
        }

        [Route("user/Forgetpassword")]
        [HttpPut]
        public async Task<IHttpActionResult> ForgetPassword(ForgetPwdInp inp)
        {          
            string email = "";
            string FullName = "";
            var users = this.AppUserManager.FindByName(inp.commonuserid);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (users == null)
            {
                return Ok("Invalid UserName");
            }
            else if (users != null)
            {
                if (users.Status == false)
                {
                    return Ok("Inactive User");
                }
            }
            if (!string.IsNullOrEmpty(inp.commonuserid))
            {
                string flname = "";
                if (!string.IsNullOrEmpty(users.LastName))
                {
                    flname = "Dear" + " " + users.FirstName + " " + users.LastName;
                }
                else
                {
                    flname = "Dear" + " " + users.FirstName;
                }

                SendVerificationEmail(users.Email, flname, users.UserName, inp.Lang);
            }
            return Ok("Success");
        }


        public string Encode(string encodeMe)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encodeMe);
            return Convert.ToBase64String(encoded);
        }


        public string Decode(string decodeMe)
        {

            byte[] encoded = Convert.FromBase64String(decodeMe);
            return System.Text.Encoding.UTF8.GetString(encoded);
        }
        public void SendVerificationEmail(string Email, string flname, string username, string lang)
        {

            try
            {
                string encryptId = Encode(username);
                string shurl = ConfigurationManager.AppSettings["UserVerificationEmail"].ToString() + "/GenerateUserPassword" + "?UserName=" + encryptId;

                string emid = ConfigurationManager.AppSettings["mailcredential"].ToString();
                string epwd = ConfigurationManager.AppSettings["mailcredentialpwd"].ToString();
                int prt = int.Parse(ConfigurationManager.AppSettings["port"].ToString());
                string hst = ConfigurationManager.AppSettings["hostaddress"].ToString();
                string mailto = Email;//ConfigurationManager.AppSettings["mailto"].ToString();
                string mailcc = ConfigurationManager.AppSettings["mailcc"].ToString();


                StringBuilder strmessagebody = new StringBuilder();
                SmtpClient sc = new SmtpClient();
                sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                sc.Credentials = new NetworkCredential(emid, epwd);
                sc.Port = prt;
                sc.Host = hst;
                MailMessage message = new MailMessage();
                //sc.EnableSsl = true;
                message.From = new MailAddress(emid, "Visa CheckOut");
                message.To.Add(mailto);
                message.CC.Add(mailcc);
                message.Subject = "Visa CheckOut  Email Verification./Visa CheckOut التحقق من البريد الإلكتروني";
                message.BodyEncoding = System.Text.Encoding.UTF8;

                StringBuilder sb = new StringBuilder();

                message.AlternateViews.Add(MailBody(username, shurl, lang));

                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                //            await sc.SendMailAsync(message);
                sc.Send(message);

            }
            catch (Exception ex)
            {

            }
        }
        private AlternateView MailBody(string fullname, string verifyLink, string lang)
        {
            string str = "";

            string s = ConfigurationManager.AppSettings["CbjImage"];
            //if (lang.ToLower() == "en")
            //{
            str = @"<table bgcolor='#fffff' width='600'>  
                <tr>
                <td>
                 " + "Dear /عزيزي " + fullname + "," + @" 
                 </td>
               </tr>
               
                <tr>  
                    <td align='center'>  
                        <img src=" + s + @" height='100px'/>                           
                    </td>  
                </tr>
                 <tr>  
                    <td align='center'> 
                        <h4>Welcome to eFawatiri / eFawatiri أهلا بك في</h4>
                    </td>  
                </tr>

                <tr>  
                    <td align='Left'> 
                        <h4>Please click on the link below to Reset your Password. /الرجاء الضغط على الرابط أدناه لإعادة تعيين كلمة السر</h4>
                    </td>  
                </tr>
                <tr>  
                   <td align='Left'>  
                    <table >  
                    <tr>  
                        <a href='" + verifyLink + @"'>Click on link /انقر على الرابط</a>
                    </tr>  
                    </table>
                    </td>  
                </tr> 
                <tr>  
                    <td align='Left'> 
                        <h4>Thank you / شكرا لك</h4>
                    </td>  
                </tr>
                <tr>  
                    <td align='Left'> 
                      <h4>EFawatiri Team / EFawatiri فريق</h4>
                    </td>  
                </tr>
            </table>";
            // }
            //else
            //{
            //    str = @"
            //<table bgcolor='#fffff' width='600' style='float:right'>  
            //    <tr>
            //    <td>
            //     " + fullname + " " + "عزيزي" + @" 
            //     </td>
            //   </tr>

            //    <tr style='float:right;'>  
            //        <td>  
            //            <img src=" + s + @" width='20px' height='20px'/>  

            //        </td>  
            //    </tr>
            //     <tr style='float:right;'>  
            //        <td > 
            //            <h4>أهلا بك في eFawatiri</h4>
            //        </td>  
            //    </tr>
            //    <tr style='float:right;'>  
            //        <td >                       
            //        </td>  
            //    </tr>

            //    <tr style='float:right;'>  
            //        <td > 
            //            <h4>الرجاء الضغط على الرابط أدناه لإعادة تعيين كلمة السر.</h4>
            //        </td>  
            //    </tr>
            //    <tr style='float:right;'>  
            //        <td >                       
            //        </td>  
            //    </tr>
            //    <tr style='float:right;'>  
            //       <td >  
            //        <table >  
            //        <tr>  
            //            <a href='" + verifyLink + @"'>انقر على الرابط</a>
            //        </tr>  
            //        </table>
            //        </td>  
            //    </tr> 
            //    <tr style='float:right;'>  
            //        <td > 
            //            <h4>شكرا لك،.</h4>
            //        </td>  
            //    </tr>
            //    <tr style='float:right;'>  
            //        <td> 
            //          <h4>فريق افاواتيري</h4>
            //        </td>  
            //    </tr>
            //</table>";
            //}
            AlternateView AV =
            AlternateView.CreateAlternateViewFromString(str, null, MediaTypeNames.Text.Html);
            //   AV.LinkedResources.Add(Img);
            return AV;
        }
    }
}
