// script.ts


    async function runScaffold() {
        const command = [
            "dotnet",
            "ef",
            "dbcontext",
            "scaffold",
            "Host=localhost;Database=DeviseHR;Username=postgres;Password=890899000",
            "Npgsql.EntityFrameworkCore.PostgreSQL",
            "-o",
            "./",
            "-f"
        ];

        const process = Deno.run({
            cmd: command,
            cwd: "../../Models", // Set this to the directory containing your .csproj file
            stdout: "piped",
            stderr: "piped",
        });

        const [stdout, stderr] = await Promise.all([
            process.output(),
            process.stderrOutput(),
        ]);

        // Decode the output
        const output = new TextDecoder().decode(stdout);
        const errorOutput = new TextDecoder().decode(stderr);

        // Log the output
        if (output) {
            console.log("Output:", output);
        }
        if (errorOutput) {
            console.error("Error:", errorOutput);
        }

        // Ensure the process has finished
        const status = await process.status();
        if (!status.success) {
            console.error("Failed to run scaffold command");
        }
    }

    runScaffold();