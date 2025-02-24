import { Link } from 'react-router-dom';

type TUser = {
  id: number;
  name: string;
  email: string;
  department: string;
};
const users: TUser[] = [
  {
    id: 1,
    name: 'Alice Johnson',
    email: 'alice@example.com',
    department: 'Engineering',
  },
  {
    id: 2,
    name: 'Bob Smith',
    email: 'bob@example.com',
    department: 'Marketing',
  },
  {
    id: 3,
    name: 'Charlie Rose',
    email: 'charlie@example.com',
    department: 'Design',
  },
  {
    id: 4,
    name: 'Diana Prince',
    email: 'diana@example.com',
    department: 'Engineering',
  },
  {
    id: 5,
    name: 'Ethan Hunt',
    email: 'ethan@example.com',
    department: 'Marketing',
  },
  {
    id: 6,
    name: 'Fiona Gallagher',
    email: 'fiona@example.com',
    department: 'Design',
  },
  {
    id: 7,
    name: 'George Costanza',
    email: 'george@example.com',
    department: 'Engineering',
  },
  {
    id: 8,
    name: 'Hannah Montana',
    email: 'hannah@example.com',
    department: 'Marketing',
  },
];

const UsersPage = () => {
  return (
    <div className="min-h-screen  bg-white dark:bg-gray-900 text-gray-900 dark:text-gray-100 p-4">
      <div className="max-w-7xl mx-auto">
        <header className="flex justify-between items-center mb-6">
          <h1 className="text-3xl font-bold">User Directory</h1>
          {/* Placeholder for future theme toggle */}
        </header>
        <section className="mb-6">
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            <input
              type="text"
              placeholder="Search by name or email"
              className="p-2 border rounded bg-white dark:bg-gray-950"
            />
            <select className="p-2 border rounded bg-white dark:bg-gray-950">
              <option>Filter by Department</option>
              <option>Engineering</option>
              <option>Marketing</option>
              <option>Design</option>
            </select>
            <div className="flex space-x-2">
              <button className="p-2 bg-pink-500 text-white rounded flex-1">
                Sort A-Z
              </button>
              <button className="p-2 bg-pink-500 text-white rounded flex-1">
                Sort Z-A
              </button>
            </div>
          </div>
        </section>
        <section>
          <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-4">
            {users.map((user: TUser) => (
              <UserCard key={user.id} user={user} />
            ))}
          </div>
        </section>
      </div>
    </div>
  );
};

function UserCard({ user }: { user: TUser }) {
  return (
    <div className="w-full max-w-sm bg-white border  border-pink-200 rounded-lg shadow-[0px_0px_13px_-9px_rgba(213,0,109,1)] dark:bg-gray-800 dark:border-gray-700">
      <div className="flex flex-col items-center mt-4 pb-6">
        <img
          className="w-24 h-24 mb-3 rounded-full shadow-lg"
          src="https://img.freepik.com/free-photo/lifestyle-people-emotions-casual-concept-confident-nice-smiling-asian-woman-cross-arms-chest-confident-ready-help-listening-coworkers-taking-part-conversation_1258-59335.jpg"
          alt={user.name}
        />
        <h5 className="mb-1 text-xl font-medium text-gray-900 dark:text-white">
          {user.name}
        </h5>
        <h5 className="mb-1 text-sm text-gray-500 dark:text-gray-400">
          {user.email}
        </h5>
        <span className="text-sm text-gray-500 dark:text-gray-400">
          {user.department}
        </span>
        <div className="flex mt-2 md:mt-4">
          <Link
            to={`/users/${user.id}`}
            className="inline-flex items-center px-4 py-2 text-sm font-medium text-center text-white bg-pink-500 rounded-lg hover:bg-pink-700 focus:ring-4 focus:outline-none "
          >
            View Profile
          </Link>
        </div>
      </div>
    </div>
  );
}

export default UsersPage;
