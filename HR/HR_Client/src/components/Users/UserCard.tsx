import { Link } from 'react-router-dom';
import { TUserCard } from '../../types/TUser';

function UserCard({ user }: { user: TUserCard }) {
  return (
    <div className="w-full max-w-sm bg-white border  border-pink-200 rounded-lg shadow-[0px_0px_13px_-9px_rgba(213,0,109,1)] dark:bg-gray-800 dark:border-gray-700">
      <div className="flex flex-col items-center mt-2 pb-3">
        <img
          className="w-24 h-24 mb-2 rounded-full shadow-lg"
          src="https://img.freepik.com/free-photo/lifestyle-people-emotions-casual-concept-confident-nice-smiling-asian-woman-cross-arms-chest-confident-ready-help-listening-coworkers-taking-part-conversation_1258-59335.jpg"
          alt={user.fullName}
        />
        <h5 className=" text-xl font-medium text-gray-900 dark:text-white">
          {user.fullName}
        </h5>
        <h5 className=" text-sm text-gray-500 dark:text-gray-400">
          {user.email}
        </h5>
        <span className="text-sm text-gray-500 dark:text-gray-400">
          {user.department}
        </span>
        <div className="flex mt-2">
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

export default UserCard;
