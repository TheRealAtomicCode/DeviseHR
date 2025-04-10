import { Link } from 'react-router-dom';
import { TUserCard } from '../../types/TUser';

function UserCard({ user }: { user: TUserCard }) {
  return (
    <div className="w-full max-w-sm bg-white dark:bg-gray-900 border border-gray-200 dark:border-gray-800 rounded-2xl shadow-md hover:shadow-lg transition-shadow duration-300 ease-in-out">
      <div className="flex flex-col items-center p-6">
        <img
          className="w-24 h-24 mb-4 rounded-full shadow-md ring-2 ring-pink-400 dark:ring-pink-500"
          src="https://img.freepik.com/free-photo/lifestyle-people-emotions-casual-concept-confident-nice-smiling-asian-woman-cross-arms-chest-confident-ready-help-listening-coworkers-taking-part-conversation_1258-59335.jpg"
          alt={user.fullName}
        />

        {user.title && (
          <p className="text-xs uppercase tracking-widest text-pink-600 dark:text-pink-400 mb-1">
            {user.title}
          </p>
        )}

        <h3 className="text-xl font-semibold text-gray-800 dark:text-white mb-1 text-center">
          {user.fullName}
        </h3>
        <p className="text-sm text-gray-500 dark:text-gray-400 text-center">
          {user.email}
        </p>
        <p className="text-sm font-medium text-pink-600 dark:text-pink-400 mt-1 text-center">
          {user.department}
        </p>
        <Link
          to={`/users/${user.id}`}
          className="mt-4 inline-flex items-center px-5 py-2 text-sm font-semibold text-white bg-pink-500 rounded-xl hover:bg-pink-600 transition-colors duration-200 focus:outline-none focus:ring-4 focus:ring-pink-300 dark:focus:ring-pink-600"
        >
          View Profile
        </Link>
      </div>
    </div>
  );
}

export default UserCard;
