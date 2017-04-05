# 博客园作业

需求说明：

  - 实现注册登录功能
  - 实现博客申请和审批功能，参照博客园的博客申请功能，审批功能就是管理员根据申请信息可以选择同意或者拒绝申请。

> 写在前面的话：
> 项目基于ASP.NET Core开发，使用MySQL数据库（PS：因为免费），开发过程以数据库优先，所以选用ADO.NET技术连接数据库。数据库的名字为cnblogsdb，转储的SQL文件cnblogsdb.sql存放在项目根目录下。
数据库

## 表结构
### blogapply表
![N|Solid](https://github.com/SyMind/CnBlogs/blob/master/demoimg/blogapply%E8%A1%A8.png)
### user表
![N|Solid](https://github.com/SyMind/CnBlogs/blob/master/demoimg/user%E8%A1%A8.png)
### email表
![N|Solid](https://github.com/SyMind/CnBlogs/blob/master/demoimg/email%E8%A1%A8.png)

## 注册功能
### 业务流程
1. 用户打开注册页面 
![N|Solid](https://github.com/SyMind/CnBlogs/blob/master/demoimg/register.png)

2.  填写表单后点击注册，若表单内容全部验证合法，将向用户的邮箱发送激活邮件
2.1注册成功后的页面（将自动跳转到用户的邮箱）
![N|Solid](https://github.com/SyMind/CnBlogs/blob/master/demoimg/registerSuccess.png)
2.2发送到邮箱的邮件
![N|Solid](https://github.com/SyMind/CnBlogs/blob/master/demoimg/registerEmail.png)
3.  用户点击邮件中的链接，或将URL复制到浏览器中打开
若URL中的参数被验证合法，则用户注册成功！否者实现错误信息，并跳转至用户帮助页面！
![N|Solid](https://github.com/SyMind/CnBlogs/blob/master/demoimg/registerFail.png)

### 细节处理
  - 点击提交按钮后，会先向服务器请求一个公钥，然后对登陆用户名和密码使用非对称加密算法加密再进行传输，保证用户安全
  - 前端的提交动作使用ajax实现，以接受后端返回的json消息渲染页面，提醒用户填写的表单信息合法性，提供给用户较好交互
> 备注
> 1.当然也可以选择HTTPS协议实现传输加密，但是浏览器受信任的根证书颁发机构的证书需要花钱，免费的证书大多浏览器都不信任
> 2.用于发送邮件的邮箱服务器相关信息存放在appGlobal.json文件中，以便根据需求修改


## 登陆功能
### 业务流程
1. 用户进入登陆页面
![N|Solid](https://github.com/SyMind/CnBlogs/blob/master/demoimg/login.png)
2. 用户填写表单，表单内容验证全部合法，将用户信息保存到Session后跳转到用户中心
3. 若用户选择下次自动登陆，将把用户密码与登陆用户名进行非对称加密后保存到Cookie中，在下次登陆时，若检测到Cookie便尝试登录

> 备注：Cookie的使用的公钥与私钥，有效时长等信息存放在appGlobal.json文件中，以便根据需求修改

## 找回密码功能
### 业务流程
1. 用户进入找回密码界面
![N|Solid](https://github.com/SyMind/CnBlogs/blob/master/demoimg/getUserName.png)

2. 若表单内容合法，则向用户邮箱发送邮件，用户名存放于邮件中
![N|Solid](https://github.com/SyMind/CnBlogs/blob/master/demoimg/getUserNameEmail.png)

## 重置密码功能
### 业务流程
1. 用户进入重置密码界面
![N|Solid](https://github.com/SyMind/CnBlogs/blob/master/demoimg/resetPassword.png)
2. 若表单内容合法，则向用户邮箱发送邮件
![N|Solid](https://github.com/SyMind/CnBlogs/blob/master/demoimg/resetPasswordEmail.png)

## 申请博客功能
### 业务流程
1. 用户进入博客申请界面
申请博客在用户没有登陆时，提醒用户登陆或注册。
![N|Solid](https://github.com/SyMind/CnBlogs/blob/master/demoimg/blogApplyPrompt.png)
2. 若填写的表单内容合法，则后端接受表单内容，用户等待申请处理
![N|Solid](https://github.com/SyMind/CnBlogs/blob/master/demoimg/blogApply.png)
3. 管理员登陆后，可以进行对博客申请进行处理
![N|Solid](https://github.com/SyMind/CnBlogs/blob/master/demoimg/blogApplyManagement.png)
4. 当管理员处理申请后，无论是同意还是拒绝均会向用户发送邮件进行提醒

> 备注
> 管理员的密码存放在appGlobal.json 中
