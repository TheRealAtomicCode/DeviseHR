export function setCookie(name: string, value: string, seconds: number) {
	const expires = new Date();
	expires.setTime(expires.getTime() + seconds * 1000);
	const expiresString = `expires=${expires.toUTCString()}`;
	document.cookie = `${name}=${encodeURIComponent(
		value
	)};${expiresString};path=/`;
}

export function getCookie(name: string): string | null {
	const nameEQ = `${name}=`;
	const cookies = document.cookie.split(';');

	for (let i = 0; i < cookies.length; i++) {
		const c = cookies[i].trim();
		if (c.startsWith(nameEQ)) {
			return decodeURIComponent(c.substring(nameEQ.length));
		}
	}

	return null;
}
