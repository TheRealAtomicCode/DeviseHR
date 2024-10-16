import { Client } from 'https://deno.land/x/postgres/mod.ts';
import { hash, genSalt } from 'https://deno.land/x/bcrypt@v0.3.0/mod.ts';
import { config } from 'https://deno.land/x/dotenv/mod.ts';

async function hashPassword(password: string): Promise<string> {
	const salt = await genSalt(10);
	const hashedPassword = await hash(password, salt);
	return hashedPassword;
}

async function insertUser(client: Client, user: any) {
	try {
		const opquery = `
      INSERT INTO Operator (first_name, last_name, email, password_hash, profile_picture, is_terminated, user_role, refresh_tokens, is_verified, added_by)
      VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10)
    `;

		const opvalues = [
			user.first_name,
			user.last_name,
			user.email,
			user.password_hash,
			user.profile_picture,
			user.is_terminated,
			user.user_role,
			user.refresh_tokens,
			true,
			1,
		];

		await client.queryObject(opquery, opvalues);

		const companiesquery = `
      INSERT INTO Company (company_name, licence_number, phone_number, expiration_date, added_by_operator, max_employees_allowed, account_number, annual_leave_start_date)
      VALUES ($1, $2, $3, $4, $5, $6, $7, $8)
      RETURNING id;
    `;

		const companyValues = [
			'DeviseHR',
			'222222',
			'2222222',
			'2024-12-31T23:59:59Z',
			1,
			20,
			'22222',
			'1970-01-01',
		];

		const company2Values = [
			'DeviseMD',
			'222221',
			'2222223',
			'2024-12-31T23:59:59Z',
			1,
			20,
			'22221',
			'1970-01-01',
		];

		const { rows } = await client.queryObject<{ id: number }>(
			companiesquery,
			companyValues
		);
		let companyId = rows[0].id;

		const userQuery = `
      INSERT INTO Employee (first_name, last_name, email, added_by_user, added_by_operator, user_role, password_hash, is_verified, company_id, annual_leave_start_date)
      VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10);
    `;

		const userValues = [
			'user1',
			'HR',
			'user1@devisehr.com',
			20,
			1,
			1,
			user.password_hash,
			true,
			companyId,
			'1970-01-01',
		];

		await client.queryObject(userQuery, userValues);
		console.log('User inserted successfully 1 company 1.');

		const user2Values = [
			'user2',
			'hr',
			'user2@devisehr.com',
			20,
			1,
			1,
			user.password_hash,
			true,
			companyId,
			'1970-01-01',
		];

		await client.queryObject(userQuery, user2Values);
		console.log('User inserted successfully 2 company 2.');

		const roo = await client.queryObject<{ id: number }>(
			companiesquery,
			company2Values
		);
		companyId = roo.rows[0].id;

		const user3Values = [
			'user1',
			'md',
			'user3@devisehr.com',
			20,
			1,
			1,
			user.password_hash,
			true,
			companyId,
			'1970-01-01',
		];

		await client.queryObject(userQuery, user3Values);
		console.log('User inserted successfully 3 company 3.');

		const user4Values = [
			'user2',
			'md',
			'user4@devisehr.com',
			20,
			1,
			1,
			user.password_hash,
			true,
			companyId,
			'1970-01-01',
		];

		await client.queryObject(userQuery, user4Values);
		console.log('User inserted successfully 4 company 3.');
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
	const plainPassword = 'password123'; // Replace with the actual plain password

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
		user: DB_USER, // Use environment variable
		password: DB_PASSWORD, // Use environment variable
		database: DB_NAME, // Use environment variable
		hostname: DB_HOST, // Use environment variable
		port: Number(DB_PORT), // Convert port to number
	};

	const client = new Client(connectionConfig);

	await client.connect();
	await insertUser(client, user);
}
