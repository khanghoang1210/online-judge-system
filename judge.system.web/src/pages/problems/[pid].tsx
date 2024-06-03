import Topbar from "@/components/Topbar/Topbar";
import Workspace from "@/components/Workspace/Workspace";
import useHasMounted from "@/hooks/useHasMounted";
import { Problem } from "@/components/ProblemsTable/ProblemsTable";
import React, { useEffect } from "react";
import Link from "next/link";

type ProblemPageProps = {
  problem: Problem;
};

const ProblemPage: React.FC<ProblemPageProps> = ({ problem }) => {
  const hasMounted = useHasMounted();
  console.log(problem);

  if (!hasMounted) {
    console.log(hasMounted);
    return null;
  }

  return (
    <div>
      <Topbar problemPage />
      <Workspace problem={problem} />
    </div>
  );
};

export default ProblemPage;

interface ProblemMap {
  [key: string]: Problem;
}

const problemMap: ProblemMap = {};
const API_URL = "http://localhost:5107/api/Problems/";

export async function getStaticPaths() {
  try {
    const res = await fetch(API_URL, {
      method: "GET",
      mode: "cors",
      headers: {
        Accept: "application/json, text/plain",
        "Content-Type": "application/json;charset=UTF-8",
      },
    });

    if (!res.ok) {
      throw new Error("Network response was not ok");
    }

    const data = await res.json();

    if (data && data.data && Array.isArray(data.data)) {
      data.data.forEach((problem: Problem) => {
        problemMap[problem.titleSlug] = problem;
      });
      const paths = Object.keys(problemMap).map((key) => ({
        params: { pid: key },
      }));

      return {
        paths,
        fallback: false,
      };
    } else {
      throw new Error("Invalid data format in the response");
    }
  } catch (err) {
    console.error("Error fetching problems:", err);
    return {
      error: true,
      message: "err.message",
    };
  }
}

export async function getStaticProps({ params }: { params: { pid: string } }) {
  const { pid } = params;
  try {
    const res = await fetch(API_URL, {
      method: "GET",
      mode: "cors",
      headers: {
        Accept: "application/json, text/plain",
        "Content-Type": "application/json;charset=UTF-8",
      },
    });

    if (!res.ok) {
      throw new Error("Network response was not ok");
    }

    const data = await res.json();

    if (data && data.data && Array.isArray(data.data)) {
      data.data.forEach((problem: Problem) => {
        problemMap[problem.titleSlug] = problem;
      });
      const problem = problemMap[pid];

      if (!problem) {
        return {
          notFound: true,
        };
      }

      return {
        props: {
          problem,
        },
      };
    } else {
      throw new Error("Invalid data format in the response");
    }
  } catch (err) {
    console.error("Error fetching problems:", err);
    return {
      error: true,
      message: "err.message",
    };
  }
}
