﻿using LiteDB;

namespace LearningWpfProject.Model
{
    public class ItemTask
    {
        public required ObjectId Id { get; set; }
        public string? Title { get; set; }

        public string? Description { get; set; }

        public bool IsCompleted { get; set; }

        public IReadOnlyList<Tag> Tags { get; set; } = [];
    }
}
