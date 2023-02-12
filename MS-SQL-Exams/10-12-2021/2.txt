INSERT INTO Passengers
SELECT FirstName + ' ' + LastName AS FullName, FirstName + LastName + '@gmail.com' AS Email FROM PILOTS
WHERE ID BETWEEN 5 AND 15