import { useState } from "react";
import { AxiosError } from "axios";
import type { ApiErrorResponse } from "../api/types";
import { authService } from "../services/authService";

export function LoginForm() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError("");

        try {
            await authService.login({ email, password });
            window.location.href = "/dashboard";
        }
        catch (err: unknown) {
            // Type guard - check if it's an Axios error
            if (err instanceof AxiosError) {
                const apiError = err.response?.data as ApiErrorResponse | undefined;
                setError(apiError?.message || err.message || "Login failed");
            } else if (err instanceof Error) {
                setError(err.message);
            } else {
                setError("An unexpected error occurred");
            }
        }
        finally {
            setLoading(false);
        }
    };

    return (
        <form onSubmit={handleLogin} className="space-y-4">
            <input 
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                placeholder="Email"
                className="w-full p-2 border rounded"
            />
            <input
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="Password"
                className="w-full p-2 border rounded"
            />
            {error && <p className="text-red-500">{error}</p>}
            <button 
                type="submit"
                disabled={loading}
                className="w-full p-2 bg-blue-500 text-white rounded hover:bg-blue-600 disabled:opacity-50"
            >
                {loading ? "Logging in..." : "Login"}
            </button>
        </form>
    );
}

