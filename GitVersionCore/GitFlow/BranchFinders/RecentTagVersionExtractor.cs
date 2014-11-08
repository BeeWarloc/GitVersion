namespace GitVersion
{
    using System.Collections.Generic;
    using System.Linq;
    using LibGit2Sharp;

    class RecentTagVersionExtractor
    {
        internal static SemanticVersionPreReleaseTag RetrieveMostRecentOptionalTagVersion(GitVersionContext context, ShortVersion matchVersion)
        {
            var tagsInDescendingOrder = context.Repository.SemVerTagsRelatedToVersion(matchVersion).OrderByDescending(tag => SemanticVersion.Parse(tag.Name));
            return RetrieveMostRecentOptionalTagVersion(context, tagsInDescendingOrder.ToList());
        }

        internal static SemanticVersionPreReleaseTag RetrieveMostRecentOptionalTagVersion(GitVersionContext context, List<Tag> applicableTagsInDescendingOrder)
        {
            if (applicableTagsInDescendingOrder.Any())
            {
                var taggedCommit = applicableTagsInDescendingOrder.First().Target;
                var preReleaseVersion = applicableTagsInDescendingOrder.Select(tag => SemanticVersion.Parse(tag.Name)).FirstOrDefault();
                if (preReleaseVersion != null)
                {
                    if (taggedCommit != context.CurrentCommit)
                    {
                        preReleaseVersion.PreReleaseTag.Number++;
                    }
                    return preReleaseVersion.PreReleaseTag;
                }
            }
            return null;
        }
    }
}