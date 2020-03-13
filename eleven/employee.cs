using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using eleven.Models;
using eleven.NewFolder;
using MySql.Data.MySqlClient;


namespace eleven
{
    public class employee
    {
        
        public appDb Db { get; }
       
        public employee(appDb db)
        {
            Db = db;
        }



        // querry execute
        public async Task<List<employees>> getUserInformation()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"select  CONCAT(firstName , ' ' , middleName , ' ' , lastname) as Name,countryName as nationality,
CONCAT(addressLine1 , ',' , addressLine2 , ' ,' , cityName,',',stateName,',',pincode)as address,GROUP_CONCAT(contact_information.contactNumber) as `numbers`,
TIMESTAMPDIFF(YEAR,dateOfJoing,CURDATE()) as experienceyaer, TIMESTAMPDIFF( MONTH, dateOfJoing, now() ) % 12 as currentExperiencemonth,
    TIMESTAMPDIFF ( YEAR, birthDate, now() ) as year
    , TIMESTAMPDIFF( MONTH, birthDate, now() ) % 12 as month
    , ( TIMESTAMPDIFF( DAY, birthDate, now() ) % 30 ) as day
FROM employee,address,join_date,city,country,contact_information,state where employee.employeeId=address.addressId and
address.cityId=city.cityId and employee.employeeId=join_date.employeeId and employee.nationality=country.countryId and state.stateId=city.stateId
 and employee.employeeId=contact_information.employeeId group by
  employee.employeeId;";
            return await ReadAllEmployee(await cmd.ExecuteReaderAsync());
        }


      
        
        //mapping 
        
        public async Task<List<employees>> ReadAllEmployee(DbDataReader reader)
        {
            var posts = new List<employees>();   // create an array of blogpost
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new employees()
                    {
                       
                        Name = reader.GetString(0),
                        nationality = reader.GetString(1),
                        Address = reader.GetString(2),
                        contactDetail = new contactInformation(reader.GetString(3)),
                        currentCompanyExp = reader.GetString(4)+ " years " + reader.GetString(5) + " months",
                        Age = new Models.dateOfBirth(reader.GetString(6) , reader.GetString(7) , reader.GetString(8)),
                       // DOJ = reader.GetString(7)

                    };
                    posts.Add(post);
                }
            }
            return posts;
        }

      
    }
}
