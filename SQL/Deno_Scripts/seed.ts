import { Client } from 'https://deno.land/x/postgres/mod.ts';
import { hash, genSalt } from 'https://deno.land/x/bcrypt@v0.3.0/mod.ts';
import { config } from 'https://deno.land/x/dotenv/mod.ts';

async function hashPassword(password: string): Promise<string> {
  const salt = await genSalt(10);
  const hashedPassword = await hash(password, salt);
  return hashedPassword;
}

async function insertUser(client: Client, user: any, passHash: string) {
  try {
    // Insert into Operator table
    const operatorQuery = `
      INSERT INTO Operator (
        first_name, 
        last_name, 
        email, 
        password_hash, 
        profile_picture, 
        is_terminated, 
        user_role, 
        refresh_tokens, 
        is_verified, 
        added_by
      )
      VALUES (
        $1, $2, $3, $4, $5, $6, $7, $8, $9, $10
      );
    `;
    const operatorValues = [
      user.first_name,
      user.last_name,
      user.email,
      passHash,
      user.profile_picture,
      user.is_terminated,
      user.user_role,
      user.refresh_tokens,
      true,  // is_verified
      1,     // added_by (assuming this is the admin or system user)
    ];

    await client.queryObject(operatorQuery, operatorValues);



	//
	//
	// companies and admins
	//
	//

	    // Insert into Company table (for company 1)
	const company1Query = `
		INSERT INTO Company (
		  company_name, 
		  licence_number, 
		  phone_number, 
		  expiration_date, 
		  added_by_operator, 
		  max_employees_allowed, 
		  account_number, 
		  annual_leave_start_date
		)
		VALUES (
		  $1, $2, $3, $4, $5, $6, $7, $8
		)
		RETURNING id;
	  `;
	  const company1Values = [
		'DeviseHR',
		'222222',
		'2222222',
		'2024-12-31T23:59:59Z',
		1,     // added_by_operator (operator id)
		20,    // max_employees_allowed
		'22222',
		'1970-01-01',  // annual_leave_start_date
	  ];
	  
	  const { rows: company1Rows } = await client.queryObject<{ id: number }>(company1Query, company1Values);
	  let companyId1 = company1Rows[0].id;

	 // Insert users for company 1
	 const adminQuery = `
	 INSERT INTO Employee (
	   first_name, 
	   last_name, 
	   email, 
	   added_by_user, 
	   added_by_operator, 
	   user_role, 
	   password_hash, 
	   is_verified, 
	   company_id, 
	   annual_leave_start_date,
	   permission_id
	 )
	 VALUES (
	   $1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11
	 );
   `;
   const adminsForCompany1 = [
	 { first_name: 'user1', last_name: 'HR', email: 'user1@devisehr.com', user_role: 50, permissionId: null },
	 { first_name: 'user2', last_name: 'hr', email: 'user2@devisehr.com', user_role: 40, permissionId: null },
   ];

   for (const user of adminsForCompany1) {
	 const userValues = [
	   user.first_name,
	   user.last_name,
	   user.email,
	   20,    // added_by_user (user id)
	   1,     // added_by_operator (operator id)
	   user.user_role,
	   passHash,
	   true,  // is_verified
	   companyId1,
	   '1970-01-01',  // annual_leave_start_date
	   user.permissionId
	 ];

	 await client.queryObject(adminQuery, userValues);
	 console.log(`User ${user.first_name} inserted successfully into company 1.`);
   }

   // Insert into Company table (for company 2)
   const company2Query = `
	 INSERT INTO Company (
	   company_name, 
	   licence_number, 
	   phone_number, 
	   expiration_date, 
	   added_by_operator, 
	   max_employees_allowed, 
	   account_number, 
	   annual_leave_start_date
	 )
	 VALUES (
	   $1, $2, $3, $4, $5, $6, $7, $8
	 )
	 RETURNING id;
   `;
   const company2Values = [
	 'DeviseMD',
	 '222221',
	 '2222223',
	 '2024-12-31T23:59:59Z',
	 1,     // added_by_operator (operator id)
	 20,    // max_employees_allowed
	 '22221',
	 '1970-01-01',  // annual_leave_start_date
   ];

   const { rows: company2Rows } = await client.queryObject<{ id: number }>(company2Query, company2Values);
   var companyId2 = company2Rows[0].id;

   // Insert users for company 2
   const adminsForCompany2 = [
	   { first_name: 'user1', last_name: 'md', email: 'user1@devisemd.com', user_role: 50, permissionId: null },
	   { first_name: 'user2', last_name: 'md', email: 'user20@devisemd.com', user_role: 40, permissionId: null },
   ];

   for (const user of adminsForCompany2) {
	 const userValues = [
	   user.first_name,
	   user.last_name,
	   user.email,
	   20,    // added_by_user (user id)
	   1,     // added_by_operator (operator id)
	   user.user_role,
	   passHash,
	   true,  // is_verified
	   companyId2,
	   '1970-01-01',  // annual_leave_start_date
	   user.permissionId
	 ];

	 await client.queryObject(adminQuery, userValues);
	 console.log(`User ${user.first_name} inserted successfully into company 2.`);
   }


	//
	//
	// permissions
	//
	//
	   // Insert permissions for company 1
	   const permissionQuery = `
		INSERT INTO public.permission (
			permission_name, 
			enable_add_employees, 
			enable_terminate_employees, 
			enable_delete_employee, 
			enable_create_pattern, 
			enable_approve_absence, 
			enable_add_manditory_leave, 
			enable_add_lateness, 
			enable_create_rotas, 
			enable_view_employee_notifications, 
			enable_view_employee_payroll, 
			enable_view_employee_sensitive_information, 
			created_at, 
			updated_at, 
			added_by, 
			updated_by, 
			company_id
		) 
		VALUES (
			$1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11, $12, $13, $14, $15, $16, $17
		);
		`;
		const permissionValues = [
		'super-manager',
		true,  // enable_add_employees
		true,  // enable_terminate_employees
		true,  // enable_delete_employee
		true,  // enable_create_pattern
		true,  // enable_approve_absence
		true,  // enable_add_mandatory_leave
		true,  // enable_add_lateness
		true,  // enable_create_rotas
		true,  // enable_view_employee_notifications
		true,  // enable_view_employee_payroll
		true,  // enable_view_employee_sensitive_information
		new Date(), // created_at (current date)
		new Date(), // updated_at (current date)
		1,     // added_by (admin/operator)
		null,  // updated_by
		1,     // company_id (for company 1)
		];
	
		await client.queryObject(permissionQuery, permissionValues);
		console.log('Permission inserted successfully for company 1.');
	
	
		// Insert permissions for company 2
		permissionValues[14] = 9; // Change company_id to 2 for company 2
		permissionValues[16] = 2; // Change company_id to 2 for company 2
		await client.queryObject(permissionQuery, permissionValues);
		console.log('Permission inserted successfully for company 2.');


//
//
// managers and employees
//
//
    // Insert users for company 1
    const userQuery = `
      INSERT INTO Employee (
        first_name, 
        last_name, 
        email, 
        added_by_user, 
        added_by_operator, 
        user_role, 
        password_hash, 
        is_verified, 
        company_id, 
        annual_leave_start_date,
		permission_id
      )
      VALUES (
        $1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11
      );
    `;
    const usersForCompany1 = [
	  { first_name: 'user3', last_name: 'hr', email: 'user3@devisehr.com', user_role: 30, permissionId: 1 },
	  { first_name: 'user4', last_name: 'hr', email: 'user4@devisehr.com', user_role: 30, permissionId: 1 },
	  { first_name: 'user5', last_name: 'hr', email: 'user5@devisehr.com', user_role: 20, permissionId: null },
	  { first_name: 'user6', last_name: 'hr', email: 'user6@devisehr.com', user_role: 20, permissionId: null },
	  { first_name: 'user7', last_name: 'hr', email: 'user7@devisehr.com', user_role: 20, permissionId: null },
	  { first_name: 'user8', last_name: 'hr', email: 'user8@devisehr.com', user_role: 20, permissionId: null },
    ];

    for (const user of usersForCompany1) {
      const userValues = [
        user.first_name,
        user.last_name,
        user.email,
        20,    // added_by_user (user id)
        1,     // added_by_operator (operator id)
        user.user_role,
        passHash,
        true,  // is_verified
        companyId1,
        '1970-01-01',  // annual_leave_start_date
		user.permissionId
      ];

      await client.queryObject(userQuery, userValues);
      console.log(`User ${user.first_name} inserted successfully into company 1.`);
    }

  

    // Insert users for company 2
    const usersForCompany2 = [
		{ first_name: 'user3', last_name: 'md', email: 'user3@devisemd.com', user_role: 30, permissionId: 1 },
		{ first_name: 'user4', last_name: 'md', email: 'user4@devisemd.com', user_role: 30, permissionId: 1 },
		{ first_name: 'user5', last_name: 'md', email: 'user5@devisemd.com', user_role: 20, permissionId: null },
		{ first_name: 'user6', last_name: 'md', email: 'user6@devisemd.com', user_role: 20, permissionId: null },
		{ first_name: 'user7', last_name: 'md', email: 'user7@devisemd.com', user_role: 20, permissionId: null },
		{ first_name: 'user8', last_name: 'md', email: 'user8@devisemd.com', user_role: 20, permissionId: null },
    ];

    for (const user of usersForCompany2) {
      const userValues = [
        user.first_name,
        user.last_name,
        user.email,
        20,    // added_by_user (user id)
        1,     // added_by_operator (operator id)
        user.user_role,
        passHash,
        true,  // is_verified
        companyId2,
        '1970-01-01',  // annual_leave_start_date
		user.permissionId
      ];

      await client.queryObject(userQuery, userValues);
      console.log(`User ${user.first_name} inserted successfully into company 2.`);
	}



	  //
	  //
	  // hierarchies
	  //
	  //
	const hierarchyQuery = `
      INSERT INTO Hierarchy (
        manager_id, 
        subordinate_id
      )
      VALUES (
        $1, $2
      );
    `;

	const hierarchiesCompany1 = [
		{ manager_id: 5, subordinate_id: 7 },
		{ manager_id: 5, subordinate_id: 8 },
		{ manager_id: 6, subordinate_id: 9 },
		{ manager_id: 6, subordinate_id: 12 },
    ];

	const hierarchiesCompany2 = [
		{ manager_id: 11, subordinate_id: 13 },
		{ manager_id: 11, subordinate_id: 14 },
		{ manager_id: 12, subordinate_id: 15 },
		{ manager_id: 12, subordinate_id: 16 },
    ];

    for (const hierarchy of hierarchiesCompany1) {
      const hierarchyQueryValues = [
        hierarchy.manager_id,
        hierarchy.subordinate_id,
      ];

      await client.queryObject(hierarchyQuery, hierarchyQueryValues);
    }
	console.log(`Herarchies added for company 1.`);

	for (const hierarchy of hierarchiesCompany2) {
		const hierarchyQueryValues = [
		  hierarchy.manager_id,
		  hierarchy.subordinate_id,
		];
  
		await client.queryObject(hierarchyQuery, hierarchyQueryValues);
	}
	console.log(`Herarchies added for company 2.`);


	//
	///
	//
	//  Working Patterns
	//
	//
	//

	const workingPatternQuery = `
      INSERT INTO working_pattern (
	  		pattern_name, company_id, added_by, 
			monday_start_time, monday_end_time,
			tuesday_start_time, tuesday_end_time,
			wednesday_start_time, wednesday_end_time,
			thursday_start_time, thursday_end_time,
			friday_start_time, friday_end_time,
			saturday_start_time, saturday_end_time,
			sunday_start_time, sunday_end_time
		)
		VALUES
		(
			$1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11, $12, $13, $14, $15, $16, $17
		);
    `;


	const workingPatternValues1 = [
		'regular employee', 1, 1, '08:00:00', '16:00:00',
		'08:00:00', '16:00:00', '08:00:00', '16:00:00', 
		'08:00:00', '16:00:00', '08:00:00', '16:00:00', 
		null, null, null, null
	  ]
	const workingPatternValues2 = [
		'regular employee', 2, 3, '08:00:00', '16:00:00',
		'08:00:00', '16:00:00', '08:00:00', '16:00:00', 
		'08:00:00', '16:00:00', '08:00:00', '16:00:00', 
		null, null, null, null
	  ]

	await client.queryObject(workingPatternQuery, workingPatternValues1);
	await client.queryObject(workingPatternQuery, workingPatternValues2);

	console.log('added working patterns for both companies')


	//
	//
	//
	// contracts
	//
	//
	//
	//


  } catch (error) {
    console.error('Error inserting user:', error);
  } finally {
    await client.end();
  }
}

export default async function seed() {
  const firstName = 'Sudo';
  const lastName = 'User';
  const email = 'sudo@devisehr.com';
  const plainPassword = 'Password123'; // Replace with the actual plain password

  const hashedPassword = await hashPassword(plainPassword);

  const user = {
    first_name: firstName,
    last_name: lastName,
    email: email,
    password_hash: hashedPassword,
    is_terminated: false,
    user_role: 1,
    profile_picture: null,
    refresh_tokens: null,
  };

  const { DB_USER, DB_PASSWORD, DB_NAME, DB_HOST, DB_PORT } = config();

  const connectionConfig = {
    user: DB_USER,
    password: DB_PASSWORD,
    database: DB_NAME,
    hostname: DB_HOST,
    port: Number(DB_PORT),
  };

  const client = new Client(connectionConfig);

  await client.connect();
  await insertUser(client, user, hashedPassword);
}
