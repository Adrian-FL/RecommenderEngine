# RecommenderEngine

A recommender engine designed to give recommendations based on users interactions.
A recommender system(engine), is a subclass of information filtering system that seeks to predict the "rating" or "preference" a user would give to an item.

The project is developed using ASP.NET CORE (.NET 6) and RavenDb. For machine learning algorithms, Accord.NET third party framework was used.

       Instructions on how to implement the database for this project: 

1.	Install RavendDb locally on your machine (v5.x).
2.	In the project, locate the folder named “RavenDbDump” and inside it, the file named “Recommender.ravendbdump”
3.	On the RavenDb interface, navigate to “Tasks”, click on “Import Database” and select “Recommender.ravendbdump”. 
        This process will create the database and along with it, will insert more than 1 milion real events and several indexes.
