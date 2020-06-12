# Gear.Notifications

[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://travis-ci.org/joemccann/dillinger)

Gear.Notifications:.

  - Gear.Notifications.Abstractions
  - Gear.Notifications
  - Gear.Common

# Features:

  - Send notifications via smtp to specific profiles

You can also:
  - CRUD actions on the notification profiles
  - CRUD actions on the Events
  - CRUD actions on the EventMarkups


### Installation

Install the nuget packages inside the project

For web applications configure the appsettings.json files by adding up the smtp settings

```sh
  "NotificationServiceSettings": {
    "Host": "smtp.office365.com",
    "EnableSsl": true,
    "Port": 666,
    "userName": "SomeMail@indrivo.com",
    "userPass": "SomePass"
  }

```

Add the settings via the options pattern
```sh
    services.Configure<NotificationServiceSettings>(Configuration.GetSection("NotificationServiceSettings"));
```

Configure the startup with the required transients
```sh
services.AddDbContext<GearNotificationsContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

    services.Configure<NotificationServiceSettings>(Configuration.GetSection("NotificationServiceSettings"));
    services.AddScoped<INotificationService, GearNotificationsService>();

    services.AddTransient<IEventImporter, EventImporterService>();
    services.AddTransient<INotificationProfileService, NotificationProfileService>();
    services.AddTransient<IHtmlEventMarkupService, HtmlEventMarkupService>();

```
Will be changed to a single Service resolver in the future
```sh
            services.GearNotificationsResolver(Configuration.GetConnectionString("DefaultConnection"), "Gear.Notifications");
```
Add Necessary migrations if needed
```sh
add-migration -Context GearNotificationsContext
update-database -Context gearNotificationsContext
```

The Gear.Notifications.Abstractions contains following dto for creating and manipulating with the services
```sh
Message, NotificationProfileModelDto, HTMLEventMakrupModelDto
```

### Usage

After creating the events, their respective markups and configuring the profiles:

  - Inject the INotificationService into the method were you want to send the notification
  - call the  INotificationService.GetUsers($eventNameParam, List<IApplication>) to get the list of emails for users who should recieve the notification
  - INotificationService.SendAsync(New Message(){
      to = ListOfEmailsDividedBy[";"]
  });




   [dill]: <https://github.com/joemccann/dillinger>
   [git-repo-url]: <https://github.com/joemccann/dillinger.git>
   [john gruber]: <http://daringfireball.net>
   [df1]: <http://daringfireball.net/projects/markdown/>
   [markdown-it]: <https://github.com/markdown-it/markdown-it>
   [Ace Editor]: <http://ace.ajax.org>
   [node.js]: <http://nodejs.org>
   [Twitter Bootstrap]: <http://twitter.github.com/bootstrap/>
   [jQuery]: <http://jquery.com>
   [@tjholowaychuk]: <http://twitter.com/tjholowaychuk>
   [express]: <http://expressjs.com>
   [AngularJS]: <http://angularjs.org>
   [Gulp]: <http://gulpjs.com>

   [PlDb]: <https://github.com/joemccann/dillinger/tree/master/plugins/dropbox/README.md>
   [PlGh]: <https://github.com/joemccann/dillinger/tree/master/plugins/github/README.md>
   [PlGd]: <https://github.com/joemccann/dillinger/tree/master/plugins/googledrive/README.md>
   [PlOd]: <https://github.com/joemccann/dillinger/tree/master/plugins/onedrive/README.md>
   [PlMe]: <https://github.com/joemccann/dillinger/tree/master/plugins/medium/README.md>
   [PlGa]: <https://github.com/RahulHP/dillinger/blob/master/plugins/googleanalytics/README.md>
