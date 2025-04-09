import { useEffect, useState } from 'react';
import UserCard from '../components/Users/UserCard';
import { TUserCard } from '../types/TUser';
import { useMutation } from '@tanstack/react-query';
import { TServiceResponse } from '../types/TServiceResponse';
import { getUsers } from '../APIs/employees/users';

// const users: TUserCard[] = [
//   {
//     id: 1,
//     name: 'Alice Johnson',
//     email: 'alice@example.com',
//     department: 'Engineering',
//   },
//   {
//     id: 2,
//     name: 'Bob Smith',
//     email: 'bob@example.com',
//     department: 'Marketing',
//   },
//   {
//     id: 3,
//     name: 'Charlie Rose',
//     email: 'charlie@example.com',
//     department: 'Design',
//   },
//   {
//     id: 4,
//     name: 'Diana Prince',
//     email: 'diana@example.com',
//     department: 'Engineering',
//   },
//   {
//     id: 5,
//     name: 'Ethan Hunt',
//     email: 'ethan@example.com',
//     department: 'Marketing',
//   },
//   {
//     id: 6,
//     name: 'Fiona Gallagher',
//     email: 'fiona@example.com',
//     department: 'Design',
//   },
//   {
//     id: 7,
//     name: 'George Costanza',
//     email: 'george@example.com',
//     department: 'Engineering',
//   },
//   {
//     id: 8,
//     name: 'Hannah Montana',
//     email: 'hannah@example.com',
//     department: 'Marketing',
//   },
// ];

const UsersPage = () => {
  const [errorMessage, setErrorMessage] = useState('');
  const [users, setUsers] = useState<TUserCard[]>([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [page, setPage] = useState(1);
  const [skip, setSkip] = useState(200);

  const { mutate, isPending } = useMutation<
    TServiceResponse<TUserCard[]>,
    Error,
    { searchTerm: string; page: number; skip: number }
  >({
    mutationFn: () => getUsers(searchTerm, page, skip),
    onSuccess: (res) => {
      if (res.success) {
        setErrorMessage('');
        setUsers(res.data);
      } else {
        setErrorMessage(res.message);
      }
    },
    onError: (error: Error) => {
      setErrorMessage(error.message);
    },
  });

  useEffect(() => {
    mutate({ searchTerm, page, skip });
  }, [mutate, searchTerm, page, skip]);

  return (
    <div className="min-h-screen  bg-white dark:bg-gray-900 text-gray-900 dark:text-gray-100 p-4">
      <div className="max-w-7xl mx-auto">
        <header className="flex justify-between items-center mb-6">
          <h1 className="text-3xl font-bold">User Directory</h1>
          {errorMessage}
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
            {users.map((user: TUserCard) => (
              <UserCard key={user.id} user={user} />
            ))}
          </div>

          {isPending ? 'Loading...' : ''}
        </section>
      </div>
    </div>
  );
};

export default UsersPage;
